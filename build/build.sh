#!/bin/bash

SKIP_PUSH=0

while getopts ":t:s" opt; do
  case $opt in
    t)
      TAG="$OPTARG"
      ;;
    s)
      SKIP_PUSH=1
      ;;
    \?)
      echo "Invalid option: -$OPTARG" >&2
      echo "USAGE: $0 -s -t <tag>"
      echo "  -s skip push to registry"
      echo "  -t build from tag specified in <tag>"
      exit 1
      ;;
  esac
done


BUILDDIR="$(dirname "$(realpath "$0")")";
SRCDIR=$(dirname "$BUILDDIR")

cd "$BUILDDIR"

echo "==> Determining TAG"
if [ -z "$TAG" ]; then
  TAG=$(git rev-list -n 1 "$(git rev-parse --abbrev-ref HEAD)")
  echo "  Determined TAG from HEAD: $TAG"
else
  echo "  Using TAG from parameter: $TAG"
fi

echo "==> Determining SHA"
SHA=$(git rev-parse "$TAG")
if [ "$?" != "0" ]; then
  echo "ERROR: could not determine SHA" >&2
  exit 1
fi

echo "==> Creating source archive"
(cd .. && ./build/archive.sh $SHA) > /dev/null
if [ "$?" != "0" ]; then
  echo "ERROR: failed to invoke (cd .. && ./build/archive.sh $SHA)" >&2
  exit 1
fi

ARCHIVE_PATH=/tmp/$SHA.tar.gz
mv ../$SHA.tar.gz "$ARCHIVE_PATH"
echo "  Created archive $ARCHIVE_PATH"

SRC_PATH=/tmp/$SHA

rm -Rf "$SRC_PATH" 2> /dev/null || true
mkdir -p $SRC_PATH

echo "  Extracting to $SRC_PATH"
tar -xf $ARCHIVE_PATH -C $SRC_PATH

VERSION="$(cat "$SRC_PATH/VERSION")"

if ! [[ "$VERSION" =~ ^([0-9]+).([0-9]+).([0-9]+)$ ]]; then
  echo "ERROR: New version tag ($VERSION) cannot be used for build" >&2
  echo "  Run ./new_version.sh to create a new tag" >&2
  exit 1
fi

BUILD=$(git rev-list "$VERSION..$TAG" --count)

echo "==> Detecting GIT params to pass to Dockerfile"
GIT_COMMIT="$(git rev-parse --short "$TAG")"
echo "    GIT_COMMIT: $GIT_COMMIT"
GIT_DESCRIBE="$(git describe "$TAG")"
echo "    GIT_DESCRIBE: $GIT_DESCRIBE"

cd "$SRC_PATH"

$BUILDDIR/patch.sh "$SRC_PATH" "$VERSION" "$BUILD"

DOCKER_IMAGE="repo.notakey.com/demo/oidc-onboarding:$VERSION"

echo "==> Building docker image from $SHA"
echo "  Creating docker image $DOCKER_IMAGE"
echo "  context: $SRC_PATH"

docker build --build-arg GIT_DESCRIBE="$GIT_DESCRIBE" --build-arg GIT_COMMIT="$GIT_COMMIT" --build-arg BUILD_DATE="$(date -u +"%Y-%m-%dT%H:%M:%SZ")" -t "$DOCKER_IMAGE" "$SRC_PATH" > /tmp/docker.build.log

if [ "$?" != "0" ]; then
    echo "ERROR: failed. See /tmp/docker.build.log"
    exit 1
fi

if [ $SKIP_PUSH -eq 0 ]; then

    if [ "$(grep -c 'repo.notakey.com' ~/.docker/config.json)" -eq 0 ]; then
        echo "==> Docker login to repo.notakey.com"
        read -p "Docker username: " DOCKER_USERNAME
        read -s -p "Docker password: " DOCKER_PASSWORD
        echo # End the line left by read -s

        docker login -u "$DOCKER_USERNAME" -p "$DOCKER_PASSWORD" repo.notakey.com
        if [ "$?" != "0" ]; then
            echo "ERROR: Docker login failed" >&2
            exit 1
        fi
    fi

    echo "==> Pushing docker image"
    docker push "$DOCKER_IMAGE" > /tmp/docker.push.log

    if [ "$?" != "0" ]; then
        echo "ERROR: push failed. See /tmp/docker.push.log"
        exit 1
    fi
fi

rm -rf "$SRC_PATH"

echo "  build successfully completed"
