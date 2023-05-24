/bin/bash

sudo curl -L "https://github.com/docker/compose/releases/download/v2.10.2/docker-compose-linux-x86_64" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

docker compose run --rm  certbot certonly --webroot --webroot-path /var/www/certbot/ --dry-run -d naudeaupetit.fr
docker compose run --rm  certbot certonly --webroot --webroot-path /var/www/certbot/ -d naudeaupetit.fr