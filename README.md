The software product provides a convenient solution for maintaining a health diary. The diary stores data on consumed products, biometrics, and physical activity.

Biometrics includes:
- Blood pressure
- Oxygen saturation
- Body temperature

These data are stored in a database, and the main difference of this health diary from a regular diary or notes is that the data is systematized and provides the user with statistical information based on the processed version, allowing them to track changes in various aspects.

The diary also features a subscription system, which, in turn, allows the user to view this statistics. The statistics include:
- Calories
- Fluid level
- Temperature

All this is calculated based on the data entered by the user in their diary and the date of entry. The statistics can be presented both in text and visually. This diary also allows users to share their data with others, such as a dietitian, fitness trainer, and others.

This project is launched through a ready-made Docker container, after which the Telegram bot is ready to perform tasks. To do this, open the terminal and enter the following commands in the specified order:
1. docker build -t healthbot
2. docker-compose build
3. docker-compose up -d

It is also necessary to obtain a token for the Telegram bot. For detailed instructions, the following link is provided: [How to get Telegram bot API token](https://www.siteguarding.com/en/how-to-get-telegram-bot-api-token).
