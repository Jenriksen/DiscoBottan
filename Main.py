import discord

TOKEN = 'NDc3OTI0NTA4NzIwOTU1NDAz.DlDN1Q.-yAj4VY12STJDypGhm30-AunpIs'

client = discord.Client()

@client.event
async def on_message(message):
    # we do not want the bot to reply to itself
    if message.author == client.user:
        return

    # Greets you.
    if message.content.startswith('!hello'):
        msg = 'Hello {0.author.mention}'.format(message)
        await client.send_message(message.channel, msg)
    
    # Lists the members of the server
    if message.content.startswith('!members'):
        memberList = [""]
        msg = ''
        for member in message.server.members:
            # Prints list to console and appends to the memberList
            print(member)
            memberList.append(member)

            # Sends message to request channel on discord in async.
            await client.send_message(message.channel, member)
        await client.send_message(message.channel, msg)
  

@client.event
async def on_ready():
    print('Logged in as')
    print(client.user.name)
    print(client.user.id)
    print('---------------------')

client.run(TOKEN)