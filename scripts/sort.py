#!/usr/bin/env python3

import os

pages_dir = "Pages"

table_lines = []
output_lines = []

def sort_key(line):
    return line.split("]", 1)[0].lower()

def add_table():
    global output_lines, table_lines
    if not table_lines:
        return
    output_lines += sorted(table_lines, key=sort_key)
    table_lines.clear()

for file_name in os.listdir(pages_dir):
    if not file_name.endswith(".md"):
        continue
    file_path = pages_dir + "/" + file_name
    output_lines = []
    input_lines = []
    with open(file_path) as f:
        in_table = False
        table_lines = []
        for line in f.readlines():
            input_lines.append(line)
            if line.startswith("|["):
                table_lines.append(line)
                continue
            # line is not a table line
            add_table()
            output_lines.append(line)
        add_table()
    if input_lines == output_lines:
        # nothing to do. file is already sorted
        continue
    print(f"sorting {file_path!r}")
    with open(file_path, "w") as f:
        f.write("".join(output_lines))
