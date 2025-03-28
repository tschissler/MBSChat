from flask import Flask, request, jsonify
from transformers import pipeline
from huggingface_hub import login

app = Flask(__name__)
login("HUGGINGFACE_TOKEN")

# Lade das Modell
generator = pipeline('text-generation', model='microsoft/Phi-3.5-mini-instruct')

@app.route('/generate', methods=['POST'])
def generate_text():
    data = request.json
    prompt = data.get('prompt', '')
    generated_text = generator(prompt, max_length=50, truncation=True, num_return_sequences=1)
    return jsonify(generated_text)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)