import os
from flask import Flask, request

port = int(os.environ.get('WEBSITE_PORT', 1234))
host = '0.0.0.0'
app = Flask(__name__)

message = 'No messages'

@app.route('/title')
def hello_world():  # put application's code here
    return {
        "title": 'Neue Nachricht: ' + message,
    }

@app.route('/title', methods=['POST'])
def post_example():
    global message
    data = request.json
    if request.is_json:
        message = data["message"]
        print(data)
        print(message)
    else:
        print ('not json')
    return data

if __name__ == '__main__':
    app.run(host=host, port=port, debug=True)
