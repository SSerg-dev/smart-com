// ----------------------------------------------------------------------
// <copyright file="UserInfo.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект SmartCom
// Версия 1.0
// Автор: Кириченко Ольга (EMail: olga.kirichenko@smart-com.su)
// ------------------------------------------------------------------------

// Класс для работы с информацией о текущем пользователе.
Ext.define('App.util.core.UserInfo', function () {
    // Объект для хранения полной информации о текущем пользователе.
    var userInfo = null;

    // Шаблон для объекта userInfo.
    var userInfoTemplate = {
        User: {
            Login: ''
        },
        CurrentRole: {
            SystemName: '',
            DisplayName: '',
            AccessPoints: [{
                Resource: '',
                Action: ''
            }]
        },
        GridInfo: {}, //TODO: дописать формат
        AuthInfo: {}, //TODO: дописать формат
        Constrains: {}
    };

    return {
        singleton: true,
        extend: 'App.AppComponent',
        alternateClassName: 'App.UserInfo',
        id: 'userinfo',
        url: 'api/Security',
        isAuthenticated: false,

        // Имя поля, в котором находятся данные.
        root: 'data',

        constructor: function (config) {
            this.addEvents('beforeload');
            this.addEvents('load');
            this.addEvents('login');

            return this.callParent(arguments);
        },

        logIn: function (login, password) {
            Ext.Ajax.request({
                method: 'POST',
                url: this.url,
                scope: this,
                params: {
                    login: login,
                    password: password
                },
                success: function (response, opts) {
                    var result = Ext.JSON.decode(response.responseText);
                    if (result.success) {
                        userInfo = readData(result[this.root]);
                        this.isAuthenticated = true;
                    }
                    this.fireEvent('login', this);
                },
                failure: function () {
                    this.isAuthenticated = false;
                    this.fireEvent('login', this);
                }
            });
        },

        logOut: function () {
            var xmlhttp;
            if (window.XMLHttpRequest) {
                xmlhttp = new XMLHttpRequest();
            } else if (window.ActiveXObject) { // code for IE
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            var url = document.location.href + '/' + this.url;
            xmlhttp.open("DELETE", url, true, "logout", "logout");
            xmlhttp.send("");
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    if (window.ActiveXObject) {
                        // IE clear HTTP Authentication
                        document.execCommand("ClearAuthenticationCache");
                    }
                    window.location.href = '';
                }
            }
        },

        // Установить текущую роль пользователя
        // (если имя роли не указано, то устанавливается роль по умолчанию).
        setCurrentRole: function (roleName) {
            this.fireEvent('beforeload', this);

            Ext.Ajax.request({
                method: 'POST',
                async: false,
                url: this.url,
                scope: this,
                urlParams: {
                    roleName: roleName || this.getCurrentRole()
                },
                success: function (response, opts) {
                    var result = Ext.JSON.decode(response.responseText);
                    if (result.success) {
                        userInfo = readData(result[this.root]);
                        this.isAuthenticated = true;
                    }
                    var recentMenuItems = Ext.ComponentQuery.query('recentmenuitems');
                    if (recentMenuItems && recentMenuItems.length > 0) {
                        recentMenuItems[0].removeAll();
                    }
                    this.fireEvent('load', this);
                },
                failure: function (response) {
                    this.fireEvent('load', this);
                }
            });
        },

        // Получить текущую роль пользователя.
        getCurrentRole: function () {
            if (!Ext.isEmpty(userInfo)) {
                return userInfo.CurrentRole;
            }
        },

        getUserName: function () {
            if (!Ext.isEmpty(userInfo)) {
                return userInfo.User.Login;
            }
        },

        hasAccessPoint: function (resource, action) {
            
            var currentRole = this.getCurrentRole();

            if (currentRole) {
                var ap = Ext.Array.findBy(currentRole.AccessPoints, function (item) {
                    return item.Resource === resource && item.Action === action && item.TPMmode === true;
                }, this);

                return ap !== null;
            }

            return false;
        },

        getAuthSourceType: function () {
            if (!Ext.isEmpty(userInfo) && !Ext.isEmpty(userInfo.AuthInfo)) {
                return userInfo.AuthInfo.Source;
            }
        },

        getGridSettings: function (xtype, mode) {
            if (!Ext.isEmpty(userInfo) && !Ext.isEmpty(userInfo.GridInfo)) {
                return userInfo.GridInfo[getKeyForGridSettings(xtype, mode)];
            }
        },

        getConstrains: function (xtype, mode) {
            if (!Ext.isEmpty(userInfo) && !Ext.isEmpty(userInfo.Constrains)) {
                return userInfo.Constrains;
            }
        },

        setGridSettings: function (xtype, mode, gridSettings) {
            var key = getKeyForGridSettings(xtype, mode),
                deferred = Q.defer();

            Ext.Ajax.request({
                method: 'POST',
                url: this.url + '/SaveGridSettings',
                scope: this,
                params: {
                    Key: key,
                    Value: Ext.JSON.encode(gridSettings)
                },
                success: function (response, opts) {
                    if (!Ext.isEmpty(userInfo) && !Ext.isEmpty(userInfo.GridInfo)) {
                        userInfo.GridInfo[key] = gridSettings;
                    }
                    return deferred.resolve(response);
                },
                failure: function () {
                    return deferred.reject();
                }
            });

            return deferred.promise;
        }
    };

    // Преобразует ответ сервера в объект userInfo.
    function readData(response) {
        return iterateTemplate(null, userInfoTemplate, response);
    }

    // Внутренний метод, преобразующий данные по шаблону.
    function iterateTemplate(key, value, response) {
        if (Ext.isArray(value)) {
            var arrayItem = value[0];
            var array = [];

            if (Ext.isArray(response)) {
                Ext.Array.each(response, function (item) {
                    var result = iterateTemplate(null, arrayItem, item);
                    if (result) {
                        array.push(result);
                    }
                });
            }

            return array;
        } else if (Ext.isObject(value)) {
            if (Ext.Object.isEmpty(value) && Ext.isObject(response)) {
                return response;
            }

            var obj = {};

            Ext.Object.each(value, function (key, value) {
                var result = iterateTemplate(key, value, response ? response[key] : null);
                obj[key] = result;
            }, this);

            return obj;
        } else {
            if (response && Ext.typeOf(response) === Ext.typeOf(value)) {
                return response;
            }

            return null;
        }
    }

    function getKeyForGridSettings(xtype, mode) {
        return Ext.String.format('{0}#{1}', xtype, mode || '');
    }
});