Ext.define('App.controller.tpm.rsmode.ConfirmUploadScenario', {
    extend: 'App.controller.core.AssociatedDirectory',
    alias: 'controller.ConfirmUploadScenarioController',

    init: function () {
        this.listen({
            component: {
                'confirmUploadScenario #confirm': {
                    click: this.onConfirmButtonClick
                },
            }
        });
    },

    onConfirmButtonClick: function (button) {
        var me = this;
        var savedScenario = button.up('confirmUploadScenario').query('#SavedScenarioId')[0];
        var savedScenarioId = savedScenario.getValue();
        if (savedScenarioId) {
            var parameters = {
                savedScenarioId: savedScenarioId
            };
            App.Util.makeRequestWithCallback('RollingScenarios', 'GetStatusScenario', parameters, function (data) {
                if (data) {
                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                    if (result.success) {
                        if (true) {
                            Ext.Msg.show({
                                title: l10n.ns('tpm', 'text').value('Confirmation'),
                                msg: result.message,
                                fn: function (btn) {
                                    if (btn === 'yes') {
                                        me.onConfirmSuccessButtonClick(button, savedScenarioId);
                                    }
                                },
                                icon: Ext.Msg.WARNING,
                                buttons: Ext.Msg.YESNO,
                                buttonText: {
                                    yes: l10n.ns('tpm', 'button').value('confirm'),
                                    no: l10n.ns('tpm', 'button').value('cancel')
                                }
                            });
                        }

                    } else {
                        me.onConfirmSuccessButtonClick(button, savedScenarioId);
                    }
                }
            }, function (data) {

            });            
        }
        else {
            App.Notify.pushError(l10n.ns('tpm', 'ConfirmUploadScenario').value('SelectSavedScenario'));
        }
    },
    onConfirmSuccessButtonClick: function (button, sScenarioId) {
        var parameters = {
            savedScenarioId: sScenarioId
        };
        button.up('confirmUploadScenario').setLoading(true);
        App.Util.makeRequestWithCallback('RollingScenarios', 'UploadScenario', parameters, function (data) {
            if (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                button.up('confirmUploadScenario').setLoading(false);
                if (result.success) {
                    var grid = Ext.ComponentQuery.query('directorygrid[name=RSmodeGrid]')[0];
                    grid.getStore().load();
                    App.Notify.pushInfo(l10n.ns('tpm', 'ConfirmUploadScenario').value('Success'));
                    button.up('window').close();                    
                } else {
                    App.Notify.pushError(l10n.ns('tpm', 'ConfirmUploadScenario').value('Error'));
                }
            }
        }, function (data) {

        });
    },
})
