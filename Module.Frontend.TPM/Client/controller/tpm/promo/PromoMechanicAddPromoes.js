Ext.define('App.controller.tpm.promo.PromoMechanicAddPromoes', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                //'promomechanicaddpromoes': {
                //    afterrender: this.onPromoesAfterRender
                //},
                'promomechanicaddpromoes[isSearch!=true] directorygrid': {
                    //extfilterchange: this.onExtFilterChange
                },
                'promomechanicaddpromoes directorygrid': {
                    beforerender: this.onPromoesGridBeforeRender,
                    afterrender: this.onPromoesGridAfterRender,
                    //selectionchange: this.onPromoesGridSelectionChange
                    //extfilterchange: this.onExtFilterChange
                },
                //'promomechanicaddpromoes directorygrid gridcolumn[cls=select-all-header]': {
                //    headerclick: this.onSelectAllRecordsClick
                //},
                'promomechanicaddpromoes #ok': {
                    click: this.onOkButtonClick
                },
            }
        });
    },
    onPromoesGridBeforeRender: function (grid) {
        var widget = grid.up('promomechanicaddpromoes');
        var store = grid.getStore();
        if (widget.PromoId) {
            var statuses = ['Approved', 'On Approval']
            store.setFixedFilter('IsGrowthAccelerationFilter', {
                operator: 'or',
                rules: [
                    {
                        operator: 'and',
                        rules: [
                            {
                                property: "IsGrowthAcceleration", operation: 'Equals', value: true
                            },
                            {
                                property: "Disabled", operation: 'Equals', value: false
                            },
                            {
                                property: 'PromoStatus.Name', operation: 'Equals', value: 'Approved'
                            },
                            {
                                property: 'ClientTreeId', operation: 'Equals', value: widget.ClientTreeId
                            },
                            {
                                property: 'StartDate', operation: 'GreaterThan', value: widget.StartDateFilter
                            },
                            {
                                operator: 'or',
                                rules: [
                                    {
                                        property: 'MasterPromoId', operation: 'Equals', value: widget.PromoId
                                    },
                                    {
                                        property: 'MasterPromoId', operation: 'Equals', value: null
                                    },
                                ]
                            },
                        ]
                    },
                    {
                        operator: 'and',
                        rules: [
                            {
                                property: "IsGrowthAcceleration", operation: 'Equals', value: false
                            },
                            {
                                property: "Disabled", operation: 'Equals', value: false
                            },
                            {
                                property: 'PromoStatus.Name', operation: 'In', value: statuses
                            },
                            {
                                property: 'ClientTreeId', operation: 'Equals', value: widget.ClientTreeId
                            },
                            {
                                property: 'Id', operation: 'NotEqual', value: widget.PromoId
                            },
                            {
                                property: 'StartDate', operation: 'GreaterThan', value: widget.StartDateFilter
                            },
                            {
                                operator: 'or',
                                rules: [
                                    {
                                        property: 'MasterPromoId', operation: 'Equals', value: widget.PromoId
                                    },
                                    {
                                        property: 'MasterPromoId', operation: 'Equals', value: null
                                    },
                                ]
                            },
                        ]
                    }

                ]
            });
        }
        else {
            var statuses = ['Approved', 'On Approval']
            store.setFixedFilter('IsGrowthAccelerationFilter', {
                operator: 'or',
                rules: [
                    {
                        operator: 'and',
                        rules: [
                            {
                                property: "IsGrowthAcceleration", operation: 'Equals', value: true
                            },
                            {
                                property: "Disabled", operation: 'Equals', value: false
                            },
                            {
                                property: 'PromoStatus.Name', operation: 'Equals', value: 'Approved'
                            },
                            {
                                property: 'ClientTreeId', operation: 'Equals', value: widget.ClientTreeId
                            },
                            {
                                property: 'MasterPromoId', operation: 'Equals', value: null
                            },
                            {
                                property: 'StartDate', operation: 'GreaterThan', value: widget.StartDateFilter
                            },
                        ]
                    },
                    {
                        operator: 'and',
                        rules: [
                            {
                                property: "IsGrowthAcceleration", operation: 'Equals', value: false
                            },
                            {
                                property: "Disabled", operation: 'Equals', value: false
                            },
                            {
                                property: 'PromoStatus.Name', operation: 'In', value: statuses
                            },
                            {
                                property: 'ClientTreeId', operation: 'Equals', value: widget.ClientTreeId
                            },
                            {
                                property: 'MasterPromoId', operation: 'Equals', value: null
                            },
                            {
                                property: 'StartDate', operation: 'GreaterThan', value: widget.StartDateFilter
                            },
                        ]
                    }
                ]
            });
        }

        var promoesGridSelectionModel = grid.getSelectionModel();

        store.on('load', function (store, records, successful) {
            if (records.length != 0 && widget.PromoId) {
                var checkedRows = new Array();
                records.forEach(function (item) {
                    if (item.data.MasterPromoId == widget.PromoId) {
                        checkedRows.push(item);
                    }
                });
                promoesGridSelectionModel.checkRows(checkedRows);
            }
            // если Promo еще не сохранен
            var promomechanic = Ext.ComponentQuery.query('promomechanic')[0];
            var linkedPromoes = promomechanic.LinkedPromoes;
            if (records.length != 0 && !widget.PromoId && linkedPromoes != null) {
                var checkedRows = new Array();
                records.forEach(function (item) {
                    if (promomechanic.LinkedPromoes.includes(item.data.Number)) {
                        checkedRows.push(item);
                    }
                });
                promoesGridSelectionModel.checkRows(checkedRows);
            }
            grid.setLoading(false);
        });

        //grid.multiSelect = true; // настройка в view
    },
    onPromoesGridAfterRender: function (grid) {
        grid.setLoading(true);
    },
    onOkButtonClick: function (button) {
        var widget = button.up('promomechanicaddpromoes');
        var promomechanic = Ext.ComponentQuery.query('promomechanic')[0];
        var grid = widget.down('grid');
        var promoStore = grid.getStore();
        var promoesGridSelectionModel = grid.getSelectionModel();
        // Все promo из стора
        var promoRecords = promoStore.getRange(0, promoStore.getTotalCount());
        var checkedRecords = promoesGridSelectionModel.getCheckedRows();

        promomechanic.LinkedPromoes = checkedRecords.map(function (item) {
            return item.data.Number;
        });
        if (promomechanic.LinkedPromoes.length > 10) {
            App.Notify.pushError('In Exchange promo can not be linked to more than 10 promoes');
            return;
        }

        if (widget.PromoId != null) {
            // Только те выбранные, что видит пользователь.
            var checkedPromoesInGrid = promoRecords.filter(function (record) {
                return checkedRecords.some(function (checkedRecord) { return record.data.Id == checkedRecord.data.Id })
            });
            // не выбранные
            var uncheckedPromoessInGrid = promoRecords.filter(function (record) {
                return !checkedRecords.includes(record)
            });
            if (checkedPromoesInGrid.length > 10) {
                App.Notify.pushError('In Exchange promo can not be linked to more than 10 promoes');
                return;
            }

            checkedPromoesInGrid.forEach(function (promo) {
                var storeproduct = promoStore.findRecord('Id', promo.data.Id);
                if (storeproduct.data.MasterPromoId != widget.PromoId) {
                    storeproduct.set('MasterPromoId', widget.PromoId);
                }
            });
            uncheckedPromoessInGrid.forEach(function (promo) {
                var storeproduct = promoStore.findRecord('Id', promo.data.Id);
                if (storeproduct.data.MasterPromoId == widget.PromoId) {
                    storeproduct.set('MasterPromoId', null)
                }
            });
            var promoRecords = promoStore.getRange(0, promoStore.getTotalCount());
            widget.setLoading(l10n.ns('core').value('savingText'));
            if (promoStore.getUpdatedRecords().length == 0) {
                widget.setLoading(false);
                widget.close();
            }
            promoStore.save({
                scope: this,
                success: function (rec, resp, opts) {
                    promomechanic.fillSelectedPromoes(null);
                    widget.setLoading(false);
                    widget.close();
                },
                failure: function () {
                    widget.setLoading(false);
                }
            });
        }
        else {
            promomechanic.fillSelectedPromoes(null);
            widget.close();
        }
    },
});