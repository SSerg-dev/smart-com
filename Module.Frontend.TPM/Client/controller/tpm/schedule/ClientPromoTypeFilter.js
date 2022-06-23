Ext.define('App.controller.tpm.schedule.ClientPromoTypeFilter', {
    extend: 'App.controller.core.AssociatedDirectory',
    alias: 'controller.clientPromoTypeFilterController',

    init: function () {
        this.listen({
            component: {
                'clientPromoTypeFilter': {
                    afterrender: this.clientPromoTypeFilterAfterrender,
                },
                'clientPromoTypeFilter #typesCheckboxes': {
                    afterrender: this.typesCheckboxesAfterrender,
                    change: this.typesFilterChange
                },
                'clientPromoTypeFilter #clientsCheckboxes': {
                    afterrender: this.clientsCheckboxesAfterrender,
                    change: this.clientFilterChange
                },
                'clientPromoTypeFilter #competitorsCheckboxes': {
                    afterrender: this.competitorsCheckboxesAfterrender,
                    change: this.competitorsFilterChange
                },
                'clientPromoTypeFilter #apply': {
                    click: this.onApplyButtonClick
                },
                'clientPromoTypeFilter #clientsFieldset': {
                    resize: this.onClientsFieldsetResize
                },
                'clientPromoTypeFilter #selectAllClients': {
                    change: this.onSelectAllClientsChange
                },
                'clientPromoTypeFilter #textFilterByClients': {
                    change: this.onTextFilterByClientsChange
                },
                'clientPromoTypeFilter #selectAllTypes': {
                    change: this.onSelectAllTypesChange
                },
                'clientPromoTypeFilter #selectAllCompetitors': {
                    change: this.onSelectAllCompetitorsChange
                },
            }
        });
    },

    onApplyButtonClick: function (button) {
        var clientsCheckboxes = button.up('clientPromoTypeFilter').down('#clientsCheckboxes');
        var typesCheckboxes = button.up('clientPromoTypeFilter').down('#typesCheckboxes');
        var competitorsCheckboxes = button.up('clientPromoTypeFilter').down('#competitorsCheckboxes');
        $('#scrollScheduler').data('jsp').scrollToY(0);
        if (clientsCheckboxes.getChecked().length == 0 || typesCheckboxes.getChecked().length == 0) {
            Ext.MessageBox.alert(l10n.ns('tpm', 'ClientPromoTypeFilter').value('Error'), l10n.ns('tpm', 'ClientPromoTypeFilter').value('SelectOneFilter'));
        } else {
            var clientsFilterConfig = Ext.ComponentQuery.query('#nascheduler')[0].clientsFilterConfig;
            var typesCheckboxesConfig = Ext.ComponentQuery.query('#nascheduler')[0].typesCheckboxesConfig;
            var competitorsCheckboxesConfig = Ext.ComponentQuery.query('#nascheduler')[0].competitorsCheckboxesConfig;
            var needReloadStore = false;
            this.filterClients(clientsCheckboxes, typesCheckboxes, competitorsCheckboxes);
            for (i = 0; i < clientsCheckboxes.items.items.length; i++) {
                if (clientsFilterConfig[i].checked != clientsCheckboxes.items.items[i].value) {
                    clientsFilterConfig[i].checked = clientsCheckboxes.items.items[i].value;
                    needReloadStore = true;
                }
            };
            for (i = 0; i < competitorsCheckboxes.items.items.length; i++) {
                if (competitorsCheckboxesConfig[i].checked != competitorsCheckboxes.items.items[i].value) {
                    competitorsCheckboxesConfig[i].checked = competitorsCheckboxes.items.items[i].value;
                    needReloadStore = true;
                }
            };
            for (i = 0; i < typesCheckboxes.items.items.length; i++) {
                if (typesCheckboxesConfig[i].checked != typesCheckboxes.items.items[i].value) {
                    typesCheckboxesConfig[i].checked = typesCheckboxes.items.items[i].value;
                    needReloadStore = true;
                }
            };
            if (needReloadStore) {
                this.getController('tpm.schedule.SchedulerViewController').onResourceStoreLoad();
            };
            var text = l10n.ns('tpm', 'Schedule').value('Filtered');
            if (button.up('clientPromoTypeFilter').down('#selectAllClients').checked
                && button.up('clientPromoTypeFilter').down('#selectAllTypes').checked
                && button.up('clientPromoTypeFilter').down('#selectAllCompetitors').checked
                && button.up('clientPromoTypeFilter').down('#textFilterByClients').hasCls('fillerText')) {
                text = l10n.ns('tpm', 'Schedule').value('AllSelected');
            };
            Ext.ComponentQuery.query('#clientsPromoTypeFilterLabel')[0].setText(text);
            button.up('window').close();
        }
    },

    isPartialyChecked: function (checkboxes) {
        var allChecked = true;
        var allUnchecked = true;
        var answer = 0;
        for (i = 0; i < checkboxes.items.items.length; i++) {
            if (!checkboxes.items.items[i].isHidden()) {
                if (!checkboxes.items.items[i].checked) {
                    allChecked = false;
                } else {
                    allUnchecked = false;
                }
            }
        }
        if (allChecked) {
            answer = 1;
        } else if (allUnchecked) {
            answer = -1;
        }
        //1 - все выбраны, 0 - часть выбрана, -1 - не выбрано ни одного
        return answer;
    },

    clientPromoTypeFilterAfterrender: function (me) {
        //me.setLoading(true);
        //var parameters = {
        //    $method: 'GET'
        //};
        ////parameters.$actionName = action;
        //breeze.EntityQuery
        //    .from('PromoTypes')
        //    .withParameters(parameters)
        //    .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
        //    .execute()
        //    .then(function (data) {
        //        me.typesCheckboxesConfig = [];
        //        data.results.forEach(function (el) {
        //            me.typesCheckboxesConfig.push({
        //                name: el.Name,
        //                inputValue: el.Name,
        //                checked: true,
        //                boxLabel: el.Name,
        //                glyph: el.Glyph,
        //                xtype: 'checkbox'
        //            })
        //        });
        //        me.setLoading(false);
        //    })
        //    .fail(function (data) {
        //        me.setLoading(false);
        //        App.Notify.pushError('Ошибка при выполнении операции');
        //    });

    },

    //CLIENTS FILTER

    clientsCheckboxesAfterrender: function (clientsCheckboxes) {
        var clientsFilterConfig = Ext.ComponentQuery.query('#nascheduler')[0].clientsFilterConfig;
        clientsCheckboxes.add(clientsFilterConfig);

        var selectAllClients = clientsCheckboxes.up('clientPromoTypeFilter').down('#selectAllClients');
        if (clientsCheckboxes.getChecked().length === clientsFilterConfig.length) {
            selectAllClients.setValue(true);
        } else {
            selectAllClients.setValue(false);
        };
    },

    clientFilterChange: function (me) {
        if (!me.up('clientPromoTypeFilter').selectAllClientsClicked) {
            var selectAllClients = me.up('clientPromoTypeFilter').down('#selectAllClients');
            if (this.isPartialyChecked(me) === 1) {
                if (selectAllClients.value != true) {
                    selectAllClients.setValue(true);
                }
            } else {
                if (selectAllClients.value != false) {
                    selectAllClients.setValue(false);
                }
            }
        }
    },

    onClientsFieldsetResize: function (me, width, height) {
        me.down('#clientsCheckboxes').setHeight(height - 100);
    },

    onSelectAllClientsChange: function (me, newValue) {
        var check = false;
        if (newValue) {
            check = true
        };
        var clientsCheckboxes = me.up('clientPromoTypeFilter').down('#clientsCheckboxes');
        if (this.isPartialyChecked(clientsCheckboxes) != 0 || check === true) {
            me.up('clientPromoTypeFilter').selectAllClientsClicked = true;
            for (i = 0; i < clientsCheckboxes.items.items.length; i++) {
                if (!clientsCheckboxes.items.items[i].isHidden()) {
                    clientsCheckboxes.items.items[i].setValue(check);
                } else {
                    //Если была фильтрация по клиенту - убираем галочку
                    clientsCheckboxes.items.items[i].setValue(false);
                };
            };
            me.up('clientPromoTypeFilter').selectAllClientsClicked = false;
        }
    },

    onTextFilterByClientsChange: function (me, newValue) {
        var clientsCheckboxes = me.up('clientPromoTypeFilter').down('#clientsCheckboxes');
        if (newValue && newValue != me.fillerText) {
            for (i = 0; i < clientsCheckboxes.items.items.length; i++) {
                if (!clientsCheckboxes.items.items[i].boxLabel.toLowerCase().startsWith(newValue.toLowerCase())) {
                    clientsCheckboxes.items.items[i].hide();
                } else {
                    clientsCheckboxes.items.items[i].show();
                }
            };
            me.focus(false);
        } else {
            for (i = 0; i < clientsCheckboxes.items.items.length; i++) {
                clientsCheckboxes.items.items[i].show();
            };
        };
        var selectAllClients = me.up('clientPromoTypeFilter').down('#selectAllClients');
        if (selectAllClients.value === true) {
            this.onSelectAllClientsChange(selectAllClients, selectAllClients.value);
        }
    },

    //private
    filterClients: function (clientsCheckboxes, typesCheckboxes, competitorsCheckboxes) {
        var me = this;
        var value = me.getFixedValue(clientsCheckboxes);
        var store = Ext.StoreMgr.lookup('MyResources');
        var typevalue = me.getTypeValuesForFilter(typesCheckboxes);
        var competitorvalue = me.getCompetitorValuesForFilter(competitorsCheckboxes);

        me.clearFilter(store);
        if (!Ext.isEmpty(value)) {
            var filter = me.createFilter(value, 'Name');
            store.filter(filter);
        }

        if (!Ext.isEmpty(typevalue)) {
            var filter = me.createFilter(typevalue, 'TypeName');
            store.filter(filter);
        }

        if (!Ext.isEmpty(competitorvalue)) {
            var filter = me.createFilter(competitorvalue, 'CompetitorName');
            store.filter(filter);
        }
    },

    createFilter: function (value, property) {
        var filterFn = function (item) {
            var re = new RegExp(value, 'i');
            return re.test(item.get(property));
        };
        var operator = 'like';
        if (Ext.isArray(value)) {
            if (value.length > 1) {
                operator = 'in';
                filterFn = function (item) {
                    var re = new RegExp('^' + value.join('|') + '$', 'i');
                    return re.test((Ext.isEmpty(property) ? me.autoStoresNullValue : item.get(property)));
                }
            } else if (value.length == 1) {
                value = value[0];
            }
        }

        var filter = Ext.create('Ext.util.Filter', {
            property: property,
            value: value,
            type: 'string',
            operator: operator,
            filterFn: filterFn
        })

        return filter;
    },

    getFixedValue: function (clientsCheckboxes) {
        var checkedArray = clientsCheckboxes.getChecked();
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

    getTypeValuesForFilter: function (typesCheckboxes) {
        var checkedArray = typesCheckboxes.getChecked();
        var value = [];
        var name;
        checkedArray.forEach(function (el) {
            name = el.name.substr(0, el.name.indexOf(' '));
            if (name == 'Loyalty' || name == 'Dynamic') {
                value.push('Other');
            } else {
                value.push(name);
            }
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

    getCompetitorValuesForFilter: function (competitorsCheckboxes) {
        var checkedArray = competitorsCheckboxes.getChecked();
        var value = [];
        checkedArray.forEach(function (el) {
            value.push(el.name);
        });

        value.push('mars');

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

    clearFilter: function (store) {
        store = store ? store : Ext.StoreMgr.lookup('MyResources');
        store.clearFilter();
    },

    //COMPETITORS FILTER

    competitorsCheckboxesAfterrender: function (me) {
        var config = Ext.ComponentQuery.query('#nascheduler')[0].competitorsCheckboxesConfig;
        me.add(config);

        var selectAllCompetitors = me.up('clientPromoTypeFilter').down('#selectAllCompetitors');
        if (me.getChecked().length === config.length) {
            selectAllCompetitors.setValue(true);
        } else {
            selectAllCompetitors.setValue(false);
        };
    },

    //filterCompetitors: function (competitorsCheckboxes) {
    //    var me = this;
    //    var value = me.getFixedValue(competitorsCheckboxes);
    //    if (Ext.isArray(value)) {
    //        for (i = 0; i < value.length; i++) {
    //            value[i] = value[i].substr(0, value[i].indexOf(' '));
    //        }
    //    } else if (value) {
    //        value = value.substr(0, value.indexOf(' '));
    //    }
    //    var store = Ext.StoreMgr.lookup('eventStore');

    //},

    onSelectAllCompetitorsChange: function (me, newValue) {
        var check = false;
        if (newValue) {
            check = true
        };
        var competitorsCheckboxes = me.up('clientPromoTypeFilter').down('#competitorsCheckboxes');
        if (this.isPartialyChecked(competitorsCheckboxes) != 0 || check === true) {
            me.up('clientPromoTypeFilter').selectAllCompetitorsClicked = true;
            for (i = 0; i < competitorsCheckboxes.items.items.length; i++) {
                if (!competitorsCheckboxes.items.items[i].isHidden()) {
                    competitorsCheckboxes.items.items[i].setValue(check);
                } else {
                    //Если была фильтрация по клиенту - убираем галочку
                    competitorsCheckboxes.items.items[i].setValue(false);
                };
            };
            me.up('clientPromoTypeFilter').selectAllCompetitorsClicked = false;
        }
    },

    competitorsFilterChange: function (me) {
        if (!me.up('clientPromoTypeFilter').selectAllCompetitorsClicked) {
            var selectAllCompetitors = me.up('clientPromoTypeFilter').down('#selectAllCompetitors');
            if (this.isPartialyChecked(me) === 1) {
                if (selectAllCompetitors.value != true) {
                    selectAllCompetitors.setValue(true);
                }
            } else {
                if (selectAllCompetitors.value != false) {
                    selectAllCompetitors.setValue(false);
                }
            }
        }
    },

    //TYPE FILTER

    typesCheckboxesAfterrender: function (me) {
        var config = Ext.ComponentQuery.query('#nascheduler')[0].typesCheckboxesConfig;
        //add type
        me.add(config);

        var selectAllTypes = me.up('clientPromoTypeFilter').down('#selectAllTypes');
        if (me.getChecked().length === config.length) {
            selectAllTypes.setValue(true);
        } else {
            selectAllTypes.setValue(false);
        };
    },

    filterTypes: function (typesCheckboxes) {
        var me = this;
        var value = me.getFixedValue(typesCheckboxes);
        if (Ext.isArray(value)) {
            for (i = 0; i < value.length; i++) {
                value[i] = value[i].substr(0, value[i].indexOf(' '));
            }
        } else if (value) {
            value = value.substr(0, value.indexOf(' '));
        }
        var store = Ext.StoreMgr.lookup('eventStore');

        if (!Ext.isEmpty(value)) {
            var filterFn = function (item) {
                var re = new RegExp(value, 'i');
                return re.test(item.get('TypeName'));
            };
            var operator = 'like';
            if (Ext.isArray(value)) {
                if (value.length > 1) {
                    operator = 'in';
                    filterFn = function (item) {
                        var re = new RegExp('^' + value.join('|') + '$', 'i');
                        return re.test((Ext.isEmpty('TypeName') ? me.autoStoresNullValue : item.get('TypeName')));
                    }
                } else if (value.length == 1) {
                    value = value[0];
                }
            }

            var filter = Ext.create('Ext.util.Filter', {
                id: 'TypeFilter',
                property: 'TypeName',
                value: value,
                type: 'string',
                operator: operator,
                filterFn: filterFn
            })
            store.removeFilter('TypeFilter');
           // store.remoteFilter = true;
            store.addFilter(filter, false);
           // store.remoteFilter = false;
            this.getController('tpm.schedule.SchedulerViewController').eventStoreLoading(store);
        } else {
            store.removeFilter('TypeFilter');
            this.getController('tpm.schedule.SchedulerViewController').eventStoreLoading(store);
        }
    },

    //filterCompetitors: function (competitorsCheckboxes) {
    //    var me = this;
    //    var checked = me.getFixedValue(competitorsCheckboxes);
    //    var value = ['mars'];

    //    if (Ext.isArray(checked))
    //        value = value.concat(checked);
    //    else
    //        value.push(checked);

    //    var store = Ext.StoreMgr.lookup('eventStore');

    //    if (!Ext.isEmpty(value)) {
    //        var filterFn = function (item) {
    //            var re = new RegExp(value, 'i');
    //            return re.test(item.get('CompetitorName'));
    //        };
    //        var operator = 'like';
    //        if (Ext.isArray(value)) {
    //            if (value.length > 1) {
    //                operator = 'in';
    //                filterFn = function (item) {
    //                    var re = new RegExp('^' + value.join('|') + '$', 'i');
    //                    return re.test((Ext.isEmpty('CompetitorName') ? me.autoStoresNullValue : item.get('CompetitorName')));
    //                }
    //            } else if (value.length == 1) {
    //                value = value[0];
    //            }
    //        }

    //        var filter = Ext.create('Ext.util.Filter', {
    //            id: 'CompetitorFilter',
    //            property: 'CompetitorName',
    //            value: value,
    //            type: 'string',
    //            operator: operator,
    //            filterFn: filterFn
    //        })
    //        store.removeFilter('CompetitorFilter');
    //        // store.remoteFilter = true;
    //        store.addFilter(filter, false);
    //        // store.remoteFilter = false;
    //        this.getController('tpm.schedule.SchedulerViewController').eventStoreLoading(store);
    //    } else {
    //        store.removeFilter('CompetitorFilter');
    //        this.getController('tpm.schedule.SchedulerViewController').eventStoreLoading(store);
    //    }
    //},

    onSelectAllTypesChange: function (me, newValue) {
        var check = false;
        if (newValue) {
            check = true
        };
        var typesCheckboxes = me.up('clientPromoTypeFilter').down('#typesCheckboxes');
        if (this.isPartialyChecked(typesCheckboxes) != 0 || check === true) {
            me.up('clientPromoTypeFilter').selectAllTypesClicked = true;
            for (i = 0; i < typesCheckboxes.items.items.length; i++) {
                if (!typesCheckboxes.items.items[i].isHidden()) {
                    typesCheckboxes.items.items[i].setValue(check);
                } else {
                    //Если была фильтрация по клиенту - убираем галочку
                    typesCheckboxes.items.items[i].setValue(false);
                };
            };
            me.up('clientPromoTypeFilter').selectAllTypesClicked = false;
        }
    },

    typesFilterChange: function (me) {
        if (!me.up('clientPromoTypeFilter').selectAllTypesClicked) {
            var selectAllTypes = me.up('clientPromoTypeFilter').down('#selectAllTypes');
            if (this.isPartialyChecked(me) === 1) {
                if (selectAllTypes.value != true) {
                    selectAllTypes.setValue(true);
                }
            } else {
                if (selectAllTypes.value != false) {
                    selectAllTypes.setValue(false);
                }
            }
        }
    },

})