The software product provides a convenient solution for maintaining a health diary. The diary stores data on consumed products and biometrics.

Biometrics includes:
- Blood pressure
- Oxygen saturation
- Heart rate

This data is stored in a postgre sql database.

The diary also features a subscription system, which, in turn, allows the user to view this statistics. The statistics include:
- Calories consumption
- Fluid consumption

All this is calculated based on the data entered by the user in their diary and the date of entry. This diary also allows users to share their data with others users of the system.

It is necessary to obtain a token for the Telegram bot. For detailed instructions, the following link is provided: [How to get Telegram bot API token](https://www.siteguarding.com/en/how-to-get-telegram-bot-api-token). After obtaining your own token you need no create Config.cs file within Configuration namespace and Config class, create token static string and continue the setup.

As this project has dockerfiles it can be starded inside contaner. To do this, if you have alredy installed docker software, open the terminal and enter the following commands in the specified order:
1. docker build -t healthbot .
2. docker-compose build
3. docker-compose up -d

But you can open it without using docker if you download postgresql and set up HealthbotContex.cs for specific instalation. After you bild and start this app, through docker or otherwise, you should be able to use it with your own bot