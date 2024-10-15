import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import { PORT } from './constants/port.constants';
import { CORS } from './constants/cors.constants';
import { DocumentBuilder, SwaggerModule } from '@nestjs/swagger';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  app.enableCors(CORS)

  const config = new DocumentBuilder()
    .setTitle("Auth Test")
    .setDescription("Test by passport, jwt and cookie")
    .setVersion("1.9")
    .build()

  const document = SwaggerModule.createDocument(app, config)
  SwaggerModule.setup("api", app, document)

  await app.listen(PORT);
  console.log(`app listen in port ${PORT}`)
}

bootstrap();
