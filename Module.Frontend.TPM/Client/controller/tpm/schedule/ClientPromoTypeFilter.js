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
                'clientPromoTypeFilter #calendarclientview': {
                    //checkchange: this.clientFilterChange
                },
                'clientPromoTypeFilter #competitorsCheckboxes': {
                    afterrender: this.competitorsCheckboxesAfterrender,
                    change: this.competitorsFilterChange
                },
                'clientPromoTypeFilter #apply': {
                    click: this.onApplyButtonClick
                },
                'clientPromoTypeFilter #clientsFieldset': {
                    //resize: this.onClientsFieldsetResize
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
        var calendarclientview = button.up('clientPromoTypeFilter').down('#calendarclientview');
        var typesCheckboxes = button.up('clientPromoTypeFilter').down('#typesCheckboxes');
        var competitorsCheckboxes = button.up('clientPromoTypeFilter').down('#competitorsCheckboxes');
        $('#scrollScheduler').data('jsp').scrollToY(0);

        if (calendarclientview.getChecked().length == 0 || typesCheckboxes.getChecked().length == 0) {
            Ext.MessageBox.alert(l10n.ns('tpm', 'ClientPromoTypeFilter').value('Error'), l10n.ns('tpm', 'ClientPromoTypeFilter').value('SelectOneFilter'));
        } else {
            var clientsFilterConfig = Ext.ComponentQuery.query('#nascheduler')[0].clientsFilterConfig;
            var typesCheckboxesConfig = Ext.ComponentQuery.query('#nascheduler')[0].typesCheckboxesConfig;
            var competitorsCheckboxesConfig = Ext.ComponentQuery.query('#nascheduler')[0].competitorsCheckboxesConfig;
            var clientNode = calendarclientview.getStore().getRootNode().getChildAt(0);
            var clientArray = [];
            clientNode.cascadeBy(function (node) {
                if (node.isLeaf()) {
                    clientArray.push({ id: node.data.objectId, name: node.data.text, checked: node.data.checked });
                };
            });
            var needReloadStore = false;
            this.filterClients(clientArray, typesCheckboxes, competitorsCheckboxes);
            clientArray.forEach(function (client) {
                var clientconfig = clientsFilterConfig.find(function (clientcfg) {
                    if (clientcfg.name == client.name) {
                        return clientcfg;
                    }                    
                });
                if (clientconfig) {
                    if (client.checked != clientconfig.checked) {
                        clientconfig.checked = client.checked;
                        needReloadStore = true;
                    }
                }
            });
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
            var checkedClientArray = clientArray.filter(function (client) {
                return client.checked == true;
            });
            if (needReloadStore) {
                this.saveSettings(checkedClientArray, typesCheckboxes, competitorsCheckboxes);
                this.getController('tpm.schedule.SchedulerViewController').onResourceStoreLoad();
            };
            var text = l10n.ns('tpm', 'Schedule').value('Filtered');
            if (button.up('clientPromoTypeFilter').down('#selectAllTypes').checked
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
        var me = this;
        var clientbaseviewstore = Ext.data.StoreManager.lookup('clientbaseviewstore');
        clientbaseviewstore.on('load', function (store, node, records, success) {
            var settingClients = me.getController('tpm.schedule.SchedulerViewController').clientSettings[0];
            var clientIds = settingClients.map(function (item) { return item.id; });
            var clientNode = store.getRootNode().getChildAt(0);
            clientNode.cascadeBy(function (node) {
                if (node.isLeaf()) {
                    if (clientIds.includes(node.data.objectId)) {
                        node.set('checked', true)
                    };
                };
            });
        })
        //clientbaseviewstore.load();
        var dddfff = clientbaseviewstore.getRootNode();
    },
    //CLIENTS FILTER

    onClientsFieldsetResize: function (me, width, height) {
        me.down('#clientbaseviewstore').setHeight(height - 100);
    },

    onTextFilterByClientsChange: function (me, newValue) {
        var calendarclientview = me.up('clientPromoTypeFilter').down('#calendarclientview');
        var clientStore = calendarclientview.getStore();
        var clientNode = clientStore.getRootNode().getChildAt(0);
        if (newValue && newValue != me.fillerText) {
            var first = true;
            clientNode.cascadeBy(function (node) {
                if (node.isLeaf()) {
                    if (node.data.text.toLowerCase().includes(newValue.toLowerCase())) {
                        if (first) {
                            calendarclientview.getSelectionModel().select([node]);
                            first = false;
                        }
                    };
                };
            });
            me.focus(false);
        } else {
            if (clientNode) {
                calendarclientview.getSelectionModel().select([clientNode]);
            }
        };
    },

    //private
    filterClients: function (clientArray, typesCheckboxes, competitorsCheckboxes) {
        var me = this;
        var value = me.getFixedValue(clientArray);
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

    getFixedValue: function (clientArray) {
        var checkedArray = clientArray.filter(function (client) {
            return client.checked == true;
        });
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
    saveSettings: function (checkedClientArray, typesCheckboxes, competitorsCheckboxes) {
        var me = this;
        var clientData = me.getClientsData(checkedClientArray);
        var typeData = me.getTypeData(typesCheckboxes);
        var competitorData = me.getCompetitorData(competitorsCheckboxes);
        var jsonarray = [];
        jsonarray.push(clientData);
        jsonarray.push(typeData);
        jsonarray.push(competitorData);
        this.getController('tpm.schedule.SchedulerViewController').clientSettings = jsonarray;
        var jsonpost = JSON.stringify(jsonarray);
        //var decodejson = Ext.JSON.decode(jsonpost);
        Ext.Ajax.request({
            method: "POST",
            url: 'api/SavedSettings/SaveSettings',
            scope: this,
            params: {
                Key: 'calendarfilter#',
                Value: jsonpost
            },
            success: function (e) {
            },
            failure: function (e) {
                App.Notify.pushError('Failure save settings');
            }
        })
    },
    getClientsData: function (checkedClientArray) {
        var value = [];
        checkedClientArray.forEach(function (el) {
            value.push({ id: el.id, name: el.name });
        });
        return value;
    },
    getTypeData: function (typesCheckboxes) {
        var checkedArray = typesCheckboxes.getChecked();
        var value = [];
        checkedArray.forEach(function (el) {
            value.push(el.name);
        });
        return value;
    },
    getCompetitorData: function (competitorsCheckboxes) {
        var checkedArray = competitorsCheckboxes.getChecked();
        var value = [];
        checkedArray.forEach(function (el) {
            value.push(el.name);
        });
        return value;
    },
})