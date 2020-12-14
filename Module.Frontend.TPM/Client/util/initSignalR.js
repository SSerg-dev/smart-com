// переопределяем поведение панели с задачами (которая внизу) дабы не править это в ядре
Ext.override(App.controller.core.loophandler.UserLoopHandler, {
    onUserLoopHandlerGridRendered: function (panel) {
        var tasksPanel = panel.up('#systempanel');
        var grid = panel.down('directorygrid');

        // не хорошо, но возможно лучше пока не придумать
        $.connection.tasksLogHub.grid = grid;

        // событие при открытии панели задач
        tasksPanel.addListener('expand', function () {
            $.connection.tasksLogHub.server.subscribeHandler();
        });

        // событие при скрытии панели задач
        tasksPanel.addListener('collapse', function () {
            $.connection.tasksLogHub.server.unsubscribeHandler();
        });
    },
});

Ext.override(App.controller.core.loophandler.LoopHandler, {
    onReadLogButtonClick: function (button) {
        var window = button.up('window');
        var record = button.up('#taskform').getRecord();

        if (record) {
            var calculatingInfoWindow = Ext.create('App.view.tpm.promocalculating.CalculatingInfoWindow', { handlerId: record.get('Id') });
            calculatingInfoWindow.down('calculatinginfolog').setLoading(true);
            calculatingInfoWindow.on({
                beforeclose: function (window) {
                    if ($.connection.tasksLogHub)
                        $.connection.tasksLogHub.server.unsubscribeLog(window.handlerId);
                }
            });
            console.log(calculatingInfoWindow);
            window.setLoading(false);
            calculatingInfoWindow.show();

            $.connection.tasksLogHub.server.subscribeLog(record.get('Id')).done(function () {
                calculatingInfoWindow.down('calculatinginfolog').setLoading(false);
            }).fail(function (reason) {
                console.log("SignalR connection failed: " + reason);
                calculatingInfoWindow.down('calculatinginfolog').setLoading(false);
            });
        }
    },
});

// проинициализировать callback-функции для хаба задач
function initSignalRTasksLog() {
    var tasksLog = $.connection.tasksLogHub; // выбираем Hub

    // без них на серверной стороне данные события работать не будут
    tasksLog.client.connected = function () { };
    tasksLog.client.disconnected = function () { };

    // функция обновления содержиомго в логе
    tasksLog.client.addInfoInLog = function (response) {
        var result = Ext.JSON.decode(response);
        var promoController = App.app.getController('tpm.promo.Promo');

        promoController.setCalculatingInformation(result);
    };

    tasksLog.client.notifyUpdateHandlers = function () {
        if ($.connection.tasksLogHub.grid)
            $.connection.tasksLogHub.grid.getStore().load();
    };
}

// проинициализировать callback-функции для хаба блокировки промо
function initSignalRPromoLog() {
    var log = $.connection.logHub; // выбираем Hub

    // без них на серверной стороне данные события работать не будут
    log.client.connected = function () { };
    log.client.disconnected = function () { };

    // функция обновления содержиомго в логе
    log.client.addInfoInLog = function (response) {
        var result = Ext.JSON.decode(response);
        var promoController = App.app.getController('tpm.promo.Promo');

        promoController.setCalculatingInformation(result);
    };

    // событие изменения блокировки промо
    log.client.changeStatusPromo = function (blocked) {
        var promoController = App.app.getController('tpm.promo.Promo');
        var windowPromo = Ext.ComponentQuery.query('promoeditorcustom');

        if (windowPromo.length > 0) {
            windowPromo = windowPromo[0];
            var record = promoController.getRecord(windowPromo);

            // разблокировать
            if (!blocked) {
                promoController.unBlockPromo(windowPromo);
            }
            else if (!record.get('Calculating')) {
                //заблокировать

                var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                var directorygrid = grid ? grid.down('directorygrid') : null;

                record.set('Calculating', true);
                windowPromo.promoId = record.data.Id;
                windowPromo.model = record;
                promoController.reFillPromoForm(windowPromo, record, directorygrid);
            }
        }
    };
};

// проинициализировать callback-функции для хаба сессии
function initSignalRPromoSession() {
    var session = $.connection.sessionHub;
    session.client.showSessionNotification = function () {
        Ext.Msg.show({
            title: l10n.ns('core').value('SessionExpiredWindowTitle'),
            msg: l10n.ns('core').value('SessionExpiredMessage'),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO,
            fn: function () {
                $.connection.hub.stop(true);
                document.location.reload(true);
            },
            closable: false
        });
    };
};

// скачиваем файл для signalR сразу при загрузке страницы
(function () {
    // проблемы из-за минификатора, можно сгененировать файл хабов сразу, но пока этого делать не будем
    $.getScript('/signalr/hubs', function () {
        initSignalRPromoSession();
        initSignalRTasksLog();
        initSignalRPromoLog();

        var disconnectedEvent = function () {
            var sessionExpiredWindowTitle = l10n.ns('core').value('SessionExpiredWindowTitle');
            if (Ext.ComponentQuery.query('[title=' + sessionExpiredWindowTitle + ']'.replace('\"', '')).length == 0) {
                Ext.Msg.show({
                    title: l10n.ns('core').value('SignalRConnectionWasLost'),
                    msg: l10n.ns('core').value('SignalRConnectionWasLostMessage'),
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.Msg.INFO,
                    fn: function () {
                        document.location.reload(true);
                    },
                    closable: false
                });
            }
        }

        $.connection.hub.start({ pingInterval: null })
        .done(function () {
            console.log('[SignalR] Connection successed!');
        })
        .fail(function () {
            disconnectedEvent(); 
        });

        $.connection.hub.connectionSlow(function() {
            console.log('[SignalR] Your connection is slow.');
        });

        $.connection.hub.reconnecting(function() {
            console.log('[SignalR] Reconnecting...');
        });

        $.connection.hub.reconnected(function() {
            console.log('[SignalR] Reconnected.');
        });

        $.connection.hub.disconnected(function() {
            console.log('[SignalR] Disconnected.');
            disconnectedEvent();
        });
    });
})();