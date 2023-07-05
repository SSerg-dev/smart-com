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
        var savedScenario = button.up('confirmUploadScenario').query('#SavedScenarioId')[0];
        var savedScenarioId = savedScenario.getValue();
        if (savedScenarioId) {
            var parameters = {
                savedScenarioId: savedScenarioId
            };
            button.up('confirmUploadScenario').setLoading(true);
            App.Util.makeRequestWithCallback('RollingScenarios', 'UploadScenario', parameters, function (data) {
                if (data) {
                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                    button.up('confirmUploadScenario').setLoading(false);
                    if (result.success) {
                        App.Notify.pushInfo(l10n.ns('tpm', 'ConfirmUploadScenario').value('Success'));
                        button.up('window').close();
                    } else {
                        App.Notify.pushError(l10n.ns('tpm', 'ConfirmUploadScenario').value('Error'));
                    }
                }
            }, function (data) {

            });
        }
        else {
            App.Notify.pushError(l10n.ns('tpm', 'ConfirmUploadScenario').value('SelectSavedScenario'));
        }
    },

})
