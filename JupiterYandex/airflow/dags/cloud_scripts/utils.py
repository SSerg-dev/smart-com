import re

def get_project_path():
    projectName = '[project_name_to_replace]'
    systemName = '[system_name_to_replace]'
    
    if re.match('\[(.*?)\]', projectName) or re.match('\[(.*?)\]', systemName):
        return ''
    else:
        return f"{projectName}/{systemName}/"