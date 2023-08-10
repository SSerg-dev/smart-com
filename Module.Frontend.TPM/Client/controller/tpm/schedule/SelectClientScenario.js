Ext.define('App.controller.tpm.schedule.SelectClientScenario', {
    extend: 'App.controller.core.AssociatedDirectory',
    alias: 'controller.selectClientScenarioController',

    init: function () {
        this.listen({
            component: {
                'selectClientScenario #clientsRadioGroup': {
                    afterrender: this.clientsRadioGroupAfterrender,
                    change: this.clientFilterChange
                },
                'selectClientScenario #apply': {
                    click: this.onApplyButtonClick
                },
                'selectClientScenario #clientsFieldset': {
                    resize: this.onClientsFieldsetResize
                },
                'selectClientScenario #textFilterByClients': {
                    change: this.onTextFilterByClientsChange
                }
            }
        });
    },

    onApplyButtonClick: function (button) {
        var clientsRadioButtons = button.up('selectClientScenario').down('#clientsRadioGroup');
        var scenarioTypeControl = button.up('selectClientScenario').down('#scenarioType');
        if (clientsRadioButtons.getChecked().length != 0) {
            var parameters = {
                ClientName: clientsRadioButtons.getChecked()[0].inputValue,
                ObjectId: clientsRadioButtons.getChecked()[0].objectId,
                ScenarioType: scenarioTypeControl.getValue()
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

    clientsRadioGroupAfterrender: function (clientsRadioGroup) {
        var clientRadios = [];
        var clientsFromFilter = JSON.parse(JSON.stringify(Ext.ComponentQuery.query('#nascheduler')[0].clientsFilterConfig));
        var availableClients = Ext.ComponentQuery.query('#nascheduler')[0].clientsAvailableForScenario;
        for (i = 0; i < clientsFromFilter.length; i++) {
            if (availableClients.indexOf(clientsFromFilter[i].objectId.toString()) > -1) {
                clientsFromFilter[i].xtype = 'radio';
                clientsFromFilter[i].checked = false;
                clientsFromFilter[i].name = 'clientRadio';
                clientRadios.push(clientsFromFilter[i]);
            }
        }
        clientsRadioGroup.add(clientRadios);
        clientsRadioGroup.suspendEvents();
    },

    onClientsFieldsetResize: function (me, width, height) {
        me.down('#clientsRadioGroup').setHeight(height - 100);
    },

    onTextFilterByClientsChange: function (me, newValue) {
        var clientsRadioButtons = me.up('selectClientScenario').down('#clientsRadioGroup');
        if (newValue && newValue != me.fillerText) {
            for (i = 0; i < clientsRadioButtons.items.items.length; i++) {
                if (!clientsRadioButtons.items.items[i].boxLabel.toLowerCase().startsWith(newValue.toLowerCase())) {
                    clientsRadioButtons.items.items[i].hide();
                } else {
                    clientsRadioButtons.items.items[i].show();
                }
            };
            me.focus(false);
        } else {
            for (i = 0; i < clientsRadioButtons.items.items.length; i++) {
                clientsRadioButtons.items.items[i].show();
            };
        };
    },

    getFixedValue: function (clientsRadioButtons) {
        var checkedArray = clientsRadioButtons.getChecked();
        var value = [];
        checkedArray.forEach(function (el) {
            value.push(el.name);
        });

        if (Ext.isArray(value)) {
            if (value.length == 0) {
                value = '';
            } else if (value.length == 1) {
                value = value[0].toLowerCase();
            }
        } else {
            value = value.toLowerCase();
        }
        return value;
    },
})
