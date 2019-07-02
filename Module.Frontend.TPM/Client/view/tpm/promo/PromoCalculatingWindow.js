Ext.define('App.view.tpm.promo.PromoCalculatingWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promocalculatingwindow',

    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoCalculatingWindow'),
    renderTo: Ext.getBody(),
    constrain: true,
    modal: true,
    width: 550,
    height: 500,
    minWidth: 550,
    minHeight: 500,
    layout: 'fit',
    jobEnded: false,
    upWindow: null,
    upGrid: null,

    items: [{
        xtype: 'textarea',
        readOnly: true
    }],

    buttons: [{
        text: l10n.ns('tpm', 'button').value('Close'),
        action: 'cancel',
        handler: function () {            
            this.up('window').close();
        }
    }],
    /*
    listeners: {
        show: function (window) {
            var promoController = App.app.getController('tpm.promo.Promo');
            var record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

            window.down('textarea').setValue('The process of getting the handler ID is in progress.');
            Ext.Ajax.request({
                method: 'POST',
                url: 'odata/Promoes/GetHandlerIdForBlockedPromo?' + 'promoId=' + record.get('Id'),
                success: function (response, opts) {
                    var result = Ext.JSON.decode(response.responseText);
                    var handlerId = result.handlerId;

                    if (handlerId) {
                        window.down('textarea').setValue('The process of getting the signalr script is in progress..');
                        $.getScript('http://localhost:8888/signalr/hubs', function () {
                            $.connection.hub.url = 'http://localhost:8888/signalr';

                            var log = $.connection.logHub;
                            log.client.sendLog = function (text) {
                                window.down('textarea').setValue(text);
                            }

                            window.down('textarea').setValue('Connection process in progress...');
                            $.connection.hub.start().done(function () {
                                log.server.bindMeWithHandler(handlerId);
                                window.down('textarea').setValue('You are connected!');
                                console.log('Connected!');

                                window.addListener('close', function () {
                                    $.connection.hub.stop();
                                    console.log('Disconnected!');
                                });
                            }).fail(function () {
                                window.down('textarea').setValue('Connection process failed.');
                            });
                        });
                    }
                    else {
                        window.setLoading(false);
                        window.down('textarea').setValue('The handler ID is not exist.');
                    }
                },
                failure: function () {
                    App.Notify.pushError('GetHandlerIdForBlockedPromo Error!');
                }
            });
        },
    }
    */
});