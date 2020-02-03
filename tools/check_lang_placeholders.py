import sys
import os.path
import re
import json

BASE_DIR = os.path.join(os.path.dirname(__file__), '..')
PROJECTS = ('HtmlRenderer', 'OverlayPlugin', 'OverlayPlugin.Common', 'OverlayPlugin.Core', 'OverlayPlugin.Updater')

TRANS_LOG_RE = re.compile(r'Log\(LogLevel\.[A-Za-z]+, Resources\.(?P<key>[A-Za-z0-9]+)(?P<params>[^\)]*)\)')
FORMAT_RE = re.compile(r'string\.Format\(Resources\.(?P<key>[A-Za-z0-9]+)(?P<params>[^\)]*)\)')
PLACEHOLD_RE = re.compile(r'\{[0-9]+\}')

if len(sys.argv) < 1:
    print('Usage: check_lang_placeholders.py')
    sys.exit(0)

with open(os.path.join(BASE_DIR, 'translations.json'), 'r', encoding='utf8') as stream:
    translations = json.load(stream)

for project in PROJECTS:
    strings = translations.get(project + '/Resources', {})

    for sub, dirs, files in os.walk(os.path.join(BASE_DIR, project)):
        for name in files:
            if name.endswith('.cs'):
                fpath = os.path.join(sub, name)

                with open(fpath, 'r', encoding='utf8') as stream:
                    data = stream.read()

                matches = []

                for m in TRANS_LOG_RE.finditer(data):
                    matches.append((m.group('key'), m.group('params')))

                for m in FORMAT_RE.finditer(data):
                    matches.append((m.group('key'), m.group('params')))

                for key, params in matches:
                    msg = strings.get(key)
                    if not msg:
                        print('ERROR: Missing key %s in %s' % (key, fpath))
                        continue

                    placholder_count = len(PLACEHOLD_RE.findall(msg['en']))
                    param_count = len(params.split(',')) - 1

                    if placholder_count != param_count:
                        print('ERROR: Found params "%s" for string %s "%s" in %s!' % (params, key, msg['en'], fpath))

    for key, trans in strings.items():
        en_placeholders = len(PLACEHOLD_RE.findall(trans['en']))

        for lang, value in trans.items():
            if lang != 'en' and not lang.startswith('#'):
                lang_placeholders = len(PLACEHOLD_RE.findall(value))

                if lang_placeholders != en_placeholders:
                    print('ERROR: Translation %s for %s in language %s has %d placeholders, expected %d!' %
                          (key, project, lang, lang_placeholders, en_placeholders))
