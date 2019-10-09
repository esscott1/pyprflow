#How to participate in the development of Pyrflow Workflow

1.  Install Visual Studio 2017 community edition
2.  git clone the master branch onto your computer
3.  create a git issue coorsponding to the work you'd like to do
4.  create local branch for that issue and develop.
5.  submit pull request into master when you are ready.

# How to see it in action and test out your changes.
Pyprflow has docker file and docker compose file that pull the latest
release verison from our private docker hub locations.
You can either install docker on your local host (docker for windows has note been
verified with this solution) or you download an Linux VDI with all the configurations 
and docker installed that is ready to go.  (The team uses this image for verification).

The location of this image is here. (tbd)

# How to publish a new release version to Dockerhub
1.  tag the release with #.#.# and push that tag to the git hub repo (any branch)
2.  this will kick off a docker hub build with docker version :release-#.#.#
an example would be image: esscott1/pyprflow:release-0.1.1
