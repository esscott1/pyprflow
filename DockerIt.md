# How to create a Docker Image for this application

There are many ways to include a Microsoft Dotnet Core API application in a Docker 
image.  This document include just one way to accomplish the goal on an U

Step 1. git pull the source code to your local machine (Linux Ubuntu 64 v16.10).

Step 2.[Install Docker](https://docs.docker.com/engine/installation/linux/ubuntu/) if you do not already have it installed.

Step 3. verify you have to Dockerfile that is include in the git repository.

Step 4. run
'''docker build -t {name of your image} .'''

Step 5. run
'''docker run -it -d -p 5000:5000 {name of your image}'''

Step 6. test it with
'''curl -i http://localhost:5000/api/workflows/example

to push to Docker Cloud you need to tag with the repository name
 '''docker tag {image name} {dockerUserName/repositoryName}:{optional tag}'''

 push to the Cloud
 '''docker push {dockerUserName/repositoryName}:{optional tag}'''
