class ProjectPathHelper:
    PROJECT_NAME="${ProjectName}"
    SYSTEM_NAME="${Environment}"

    def getPath():
        #exception will raise only if deploy pipeline won't process this file
        raise Exception("Failed to initialize project path")
        return f"{ProjectPathHelper.PROJECT_NAME}/{ProjectPathHelper.SYSTEM_NAME}/"
    
    def getDagId():
        #exception will raise only if deploy pipeline won't process this file
        raise Exception("Failed to initialize project path")
        return f"{ProjectPathHelper.PROJECT_NAME}_{ProjectPathHelper.SYSTEM_NAME}_"