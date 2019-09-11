//l10n.buildLocalization();
Ext.setGlyphFontFamily('MaterialDesignIcons');

Ext.application({
    name: 'App',

    controllers: ResourceMgr.getControllerList(),

    launch: function () {
        l10n.clearTempData();
        ResourceMgr.clearTempData();
        Ext.FocusManager.enable({ focusFrame: false });
        this.setGlobalHandlers();
        //Ext.widget('loginwindow').show();
        App.UserInfo.setCurrentRole();
        //Ext.Date.defaultFormat = 'd.m.Y';
        Ext.create('App.view.core.Viewport');
        KeysMngr.bindKeys();
    },       

    setGlobalHandlers: function () {
        Ext.util.Observable.observe(Ext.data.Connection);
        Ext.util.Observable.observe(Ext.ux.data.proxy.Breeze);
        Ext.data.Connection.on('requestexception', function (conn, response) {
            console.log('Ajax request error: ', arguments);

            if (response && response.status === 401) {
                App.UserInfo.isAuthenticated = false;
            } else {
                App.Notify.pushError('Не удалось получить ответ от сервера.');
            }
        });
        Ext.data.Connection.on('requestcomplete', function (conn, response) {
            console.log('Ajax request complete: ', arguments);
            var data = Ext.JSON.decode(response.responseText, true);

            if (data && data.success === false) {
                if (data.message == "SESSION_EXPIRED") {
                    /*
                    Ext.Msg.show({
                        title: l10n.ns('core').value('SessionExpiredWindowTitle'),
                        msg: l10n.ns('core').value('SessionExpiredMessage'),
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.Msg.INFO,
                        fn: function () {
                            document.location.reload(true);
                        },
                        cls: 'over_all',
                        closable: false
                    });
                    */
                } else {
                    App.Notify.pushError(data.message);
                }
            }
        });
        Ext.ux.data.proxy.Breeze.on('exception', function (proxy, response, operation) {
            console.log('Breeze request error: ', arguments);
            if (!operation.wasSuccessful()) {
                var error = operation.getError(),
                    message = "Unknown breeze error";

                if (Ext.isString(error)) {
                    message = error;
                } else if (error.body) {
                    if (error.body['odata.error']) {
                        message = error.body['odata.error'].innererror.message;
                    } else if (error.body.ExceptionMessage) {
                        message = error.body.ExceptionMessage;
                    }
                } else {
                    message = error.message;
                }

                App.Notify.pushError(message);
            }
        });
    }

});