import re

def get_project_path():
    projectName = '${ProjectName}'
    systemName = '${Environment}'
    
    if re.match('\[(.*?)\]', projectName) or re.match('\[(.*?)\]', systemName):
        return ''
    else:
        return f"{projectName}/{systemName}/"