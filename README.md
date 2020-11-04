Turnike WebApi Scalable Service
=========

A simple distributed application running across multiple Docker containers.

Getting started
---------------

Download [Docker Desktop](https://www.docker.com/products/docker-desktop) for Mac or Windows. [Docker Compose](https://docs.docker.com/compose) will be automatically installed. On Linux, make sure you have the latest version of [Compose](https://docs.docker.com/compose/install/). 

## Linux Containers

The Linux stack uses Nginx, Asp NET Core with Postgres for storage.

> If you're using [Docker Desktop on Windows](https://store.docker.com/editions/community/docker-ce-desktop-windows), you can run the Linux version by [switching to Linux containers](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers), or run the Windows containers version.

Run in this directory:
```
docker-compose up
```
The app will be running at [http://localhost:80](http://localhost:80), and the results will be at [http://localhost:80](http://localhost:80).

Alternately, if you want to run it on a [Docker Swarm](https://docs.docker.com/engine/swarm/), first make sure you have a swarm. If you don't, run:
```
docker swarm init
```
Once you have your swarm, in this directory run:
```
docker stack deploy --compose-file docker-stack.yaml turnike
```

## Configuration

Database password in docker-compose.yaml and docker-stack.yaml 8th line.
If you want to change to database configuratin can be found on build/turnike-webapi/appsettins.json 15th line.

## Nginx

Nginx services implement reverse proxy to webapi
