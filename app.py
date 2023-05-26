from flask import Flask

port = 1234
host = '127.0.0.1'
app = Flask(__name__)


@app.route('/helloworld')
def hello_world():  # put application's code here
    return 'Hello World!'


if __name__ == '__main__':
    app.run(host=host, port=port)
