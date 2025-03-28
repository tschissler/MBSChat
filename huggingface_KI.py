# docker run -it huggingface/transformers-pytorch-gpu bash

from huggingface_hub import login
from transformers import pipeline

# Gehe zu Hugging Face.
# Melde dich an oder erstelle ein Konto.
# Gehe zu deinem Profil und klicke auf "Settings".
# Wähle "Access Tokens" und erstelle ein neues Token.
login("HUGGINGFACE_TOKEN")
generator = pipeline('text-generation', model='microsoft/Phi-3.5-mini-instruct')
generated = generator("Was ist die beste KI?", num_return_sequences=1)
print(generated)