# Rebels.io

I would like to leave the following notes about my solution:

- For storing the data, I'm using EF in memory DB;
- When storing a new show and its cast, I first check if the actor/actress with the same id already exists in storage;
- There's the entity ShowCast that relates a multiple shows to multiple people (the cast), making a many to many relationship;

Using the API:

The endpoint receives the page number, each page size is hardcoded at the controller to 10.

Example (considering it's running with default port):

https://localhost:5001/api/Shows/1 (page 1, first 10 shows)

If you have any questions, please, email me and I'll answer as soon as I can.

Thank you for your time.
