Ext.define('App.util.core.MenuFunctionManager', {
    singleton: true,

    functionConfig: undefined,

    initFunctionConfig: function (force) {
        if (force || !this.functionConfig) {
            this.functionConfig = {
                'core': {
                    'downloadManual': this.downloadManual
                },
                'd2s': {},
            };
        }
    },



    getMenuFunction: function (funcName) {
        this.initFunctionConfig();
        var func = this.parseFuncName(funcName);
        if (func) {
            var module = this.functionConfig[func.moduleName];
            if (module) {
                return module[func.functionName];
            } else {
                console.warn('Module \'' + func.moduleName + '\' not found');
            }
        } else {
            console.warn('Can\'t parse funcName \'' + funcName + '\'');
            return null;
        }
    },

    parseFuncName: function (funcName) {
        if (funcName) {
            var arr = funcName.split('.');
            if (arr.length == 0) {
                return {
                    moduleName: 'core',
                    functionName: arr[0]
                }
            } else if (arr.length > 1) {
                return {
                    moduleName: arr[0],
                    functionName: arr[1]
                }
            }
        } else {
            console.warn('funcName is empty');
            return null;
        }
    },

    downloadManual: function (button) {
        if (button.downloadUrl) {
            App.util.core.Util.downloadFile({
                url: button.downloadUrl,
                method: 'GET',
                params: button.downloadParams
            });
        } else {
            console.warn('manual url is empty');
        }
    }
});