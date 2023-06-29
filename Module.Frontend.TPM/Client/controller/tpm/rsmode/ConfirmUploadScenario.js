Ext.define('App.controller.tpm.rsmode.ConfirmUploadScenario', {
    extend: 'App.controller.core.AssociatedDirectory',
    alias: 'controller.ConfirmUploadScenarioController',

    init: function () {
        this.listen({
            component: {
                'selectClientScenario #confirm': {
                    click: this.onConfirmButtonClick
                },
            }
        });
    },

    onConfirmButtonClick: function (button) {
        var clientsRadioButtons = button.up('selectClientScenario').down('#clientsRadioGroup');
        if (clientsRadioButtons.getChecked().length != 0) {
            var parameters = {
                ClientName: clientsRadioButtons.getChecked()[0].inputValue,
                ObjectId: clientsRadioButtons.getChecked()[0].objectId
            };
            button.up('selectClientScenario').setLoading(true);
            App.Util.makeRequestWithCallback('ClientTrees', 'SaveScenario', parameters, function (data) {
                if (data) {
                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                    button.up('selectClientScenario').setLoading(false);
                    if (result.success) {
                        App.Notify.pushInfo(l10n.ns('tpm', 'Schedule').value('SaveScenarioTaskCreated'));
                        button.up('window').close();
                        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                        if (scheduler) {
                            scheduler.baseClientsStore.reload();
                        }
                    } else {
                        App.Notify.pushError(l10n.ns('tpm', 'Schedule').value('SaveScenarioTaskError'));
                    }
                }
            }, function (data) {

            });
        }
        else {
            App.Notify.pushError(l10n.ns('tpm', 'Schedule').value('SaveScenarioSelectError'));
        }
    },

})
