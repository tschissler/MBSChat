#!/bin/bash

# Fix difference in CRLF on Windows and Linux
git config --global core.autocrlf input

# Install requirements
pip install --upgrade pip
pip3 install --user -r requirements.txt
