Ext.define('App.controller.tpm.schedule.SelectClientYearCopy', {
    extend: 'App.controller.core.AssociatedDirectory',
    alias: 'controller.selectClientYearCopyController',

    init: function () {
        this.listen({
            component: {
                'selectClientYearCopy #clientsRadioGroup': {
                    afterrender: this.clientsRadioGroupAfterrender,
                    change: this.clientFilterChange
                },
                'selectClientYearCopy #apply': {
                    click: this.onApplyButtonClick
                },
                'selectClientYearCopy #clientsFieldset': {
                    resize: this.onClientsFieldsetResize
                },
                'selectClientYearCopy #textFilterByClients': {
                    change: this.onTextFilterByClientsChange
                }
            }
        });
    },

    onApplyButtonClick: function (button) {
        var me = this;
        var clientsRadioButtons = button.up('selectClientYearCopy').down('#clientsRadioGroup');
        var parameters = {
            objectId: clientsRadioButtons.getChecked()[0].objectId
        };
        App.Util.makeRequestWithCallback('RollingScenarios', 'GetStatusScenario', parameters, function (data) {
            if (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    if (!result.gogs) {
                        Ext.Msg.show({
                            title: l10n.ns('tpm', 'text').value('Confirmation'),
                            msg: result.message,
                            fn: function (btn) {
                                if (btn === 'yes') {
                                    me.onConfirmSuccessButtonClick(button);
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
                    else {
                        debugger;
                        App.Notify.pushError(result.message);
                    }
                } else {
                    me.onConfirmSuccessButtonClick(button);
                }
            }
        }, function (data) {

        });
        
    },
    onConfirmSuccessButtonClick: function (button) {
        var clientsRadioButtons = button.up('selectClientYearCopy').down('#clientsRadioGroup');
        var checkedDate = button.up('selectClientYearCopy').down('#checkboxDate');
        if (clientsRadioButtons.getChecked().length != 0) {
            var parameters = {
                ObjectId: clientsRadioButtons.getChecked()[0].objectId,
                CheckedDate: checkedDate.checked
            };
            button.up('selectClientYearCopy').setLoading(true);
            App.Util.makeRequestWithCallback('ClientTrees', 'CopyYearScenario', parameters, function (data) {
                if (data) {
                    var result = Ext.JSON.decode(data.httpResponse.data.value);
                    button.up('selectClientYearCopy').setLoading(false);
                    if (result.success) {
                        App.Notify.pushInfo(l10n.ns('tpm', 'SelectClientYearCopy').value('SaveYearCopyTaskCreated'));
                        button.up('window').close();
                        App.System.openUserTasksPanel();
                        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                        if (scheduler) {
                            scheduler.baseClientsStore.reload();
                        }
                    } else {
                        App.Notify.pushError(l10n.ns('tpm', 'SelectClientYearCopy').value('SaveYearCopyTaskError'));
                    }
                }
            }, function (data) {

            });
        }
        else {
            App.Notify.pushError(l10n.ns('tpm', 'SelectClientYearCopy').value('SaveYearCopySelectError'));
        }
    },
    clientsRadioGroupAfterrender: function (clientsRadioGroup) {
        var clientRadios = [];
        var clientsFromFilter = JSON.parse(JSON.stringify(Ext.ComponentQuery.query('#nascheduler')[0].clientsFilterConfig));
        for (i = 0; i < clientsFromFilter.length; i++) {
            clientsFromFilter[i].xtype = 'radio';
            clientsFromFilter[i].checked = false;
            clientsFromFilter[i].name = 'clientRadio';
            clientRadios.push(clientsFromFilter[i]);
        }
        clientsRadioGroup.add(clientRadios);
        clientsRadioGroup.suspendEvents();
    },

    onClientsFieldsetResize: function (me, width, height) {
        me.down('#clientsRadioGroup').setHeight(height - 100);
    },

    onTextFilterByClientsChange: function (me, newValue) {
        var clientsRadioButtons = me.up('selectClientYearCopy').down('#clientsRadioGroup');
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
