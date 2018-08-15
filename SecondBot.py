import random
import asyncio
import aiohttp
import requests
import json
from discord import game
from discord.ext.commands import Bot

BOT_PREFIX =("?","!")

TOKEN = "NDc3OTI0NTA4NzIwOTU1NDAz.DlDN1Q.-yAj4VY12STJDypGhm30-AunpIs"

client = Bot(command_prefix=BOT_PREFIX)

@client.command(name='8Ball',
 description="Answers a yes/no question.",
  brief="Answers from the beyond.",
   aliases=['eight_ball',
   '8-ball'],
   pass_context=True)
async def eight_ball(context):
    possible_responses = [
        'That is a resounding no',
        'It is not looking likely',
        'Too hard to tell',
        'It is quite possible',
        'Definitely',
    ]
    await client.say(random.choice(possible_responses) + ", " + context.message.author.mention)

@client.command()
async def square(number):
    squared_value= int(number)*int(number)
    await client.say(str(number)+ " squared is " + str(squared_value))

# @client.event
# async def on_ready():
#     await client.change_presence(game=Game(name="with humans"))
#     print("Logged in as " + client.user.name)

@client.command()
async def bitcoin():
    url = 'httpsS://api.coindesk.com/v1/bpi/currentprice/BTC.json'
    async with aiohttp.ClientSession() as session: #async http request
        raw_response = await session.get(url)
        response = await raw_response.text()
        response = json.loads(response)
        await client.say("Bitcoin price is: $" + response['bpi']['USD']['rate'])

# background task example
async def list_servers():
    await client.wait_until_ready()
    while not client.is_closed:
        print("Current servers:")
        for server in client.servers:
            print(server.name)
        await asyncio.sleep(6)

client.loop.create_task(list_servers())

client.run(TOKEN)