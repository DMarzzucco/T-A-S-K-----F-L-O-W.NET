# API AUTH SECURITY IMPLEMENTS
Test API, to test validation and authentication systems.

## Install

```bash
#Init the db
docker-compose up db 

#Generate the table
npx prisma generate

# Migrate the deploy
npx prisma migrate deploy

# Start the server
npm run start:dev

```

## Documentation Swagger

[Docs](http://localhost:3003/docs)

## Author
Dario Marzzucco 