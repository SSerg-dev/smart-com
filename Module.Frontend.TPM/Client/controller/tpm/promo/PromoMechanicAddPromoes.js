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
        //widget.setLoading(true);
        if (widget.IsGrowthAcceleration) {
            store.setFixedFilter('IsGrowthAccelerationFilter', {
                operator: 'and',
                rules: [
                    //{
                    //    operator: 'or',
                    //    rules: [
                    //        {
                    //            property: 'MasterPromoId',
                    //            operation: 'Equal',
                    //            value: widget.PromoId
                    //        },
                    //        {
                    //            property: 'MasterPromoId',
                    //            operation: 'Equal',
                    //            value: null
                    //        },
                    //    ]
                    //},
                    {
                        property: "IsGrowthAcceleration",
                        operation: 'Equals',
                        value: widget.IsGrowthAcceleration
                    },
                    {
                        property: 'PromoStatus.Name',
                        operation: 'Equals',
                        value: 'Approved'
                    },
                    {
                        property: 'ClientTreeId',
                        operation: 'Equals',
                        value: widget.ClientTreeId
                    },
                    {
                        property: 'Id',
                        operation: 'NotEqual',
                        value: widget.PromoId
                    }
                ]
            });
        }
        else {
            var statuses = ['Approved', 'On Approval']
            store.setFixedFilter('IsGrowthAccelerationFilter', {
                operator: 'and',
                rules: [
                    //{
                    //    operator: 'or',
                    //    rules: [
                    //        {
                    //            property: 'MasterPromoId',
                    //            operation: 'Equal',
                    //            value: widget.PromoId
                    //        },
                    //        {
                    //            property: 'MasterPromoId',
                    //            operation: 'Equal',
                    //            value: null
                    //        },
                    //    ]
                    //},
                    {
                        property: "IsGrowthAcceleration",
                        operation: 'Equals',
                        value: widget.IsGrowthAcceleration
                    },
                    {
                        property: 'PromoStatus.Name',
                        operation: 'In',
                        value: statuses
                    },
                    {
                        property: 'ClientTreeId',
                        operation: 'Equals',
                        value: widget.ClientTreeId
                    },
                    {
                        property: 'Id',
                        operation: 'NotEqual',
                        value: widget.PromoId
                    }
                ]
            });
        }
        var promoesGridSelectionModel = grid.getSelectionModel();
        
        store.on('load', function (store, records, successful) {
            if (records.length != 0) {
                var checkedRows = new Array();
                records.forEach(function (item) {
                    if (item.data.MasterPromoId == widget.PromoId) {
                        checkedRows.push(item);
                    }

                });
                promoesGridSelectionModel.checkRows(checkedRows);
                grid.setLoading(false);
            }
        });

        //grid.multiSelect = true; // настройка в view
    },
    onPromoesGridAfterRender: function (grid) {
        grid.setLoading(true);
    },
    onOkButtonClick: function (button) {
        var widget = button.up('promomechanicaddpromoes');
        var grid = widget.down('grid');
        var promoStore = grid.getStore();
        var promoesGridSelectionModel = grid.getSelectionModel();
        // Все promo из стора
        var promoRecords = promoStore.getRange(0, promoStore.getTotalCount());
        var checkedRecords = promoesGridSelectionModel.getCheckedRows();
        // Только те выбранные, что видит пользователь.
        var checkedPromoesInGrid = promoRecords.filter(function (record) {
            return checkedRecords.some(function (checkedRecord) { return record.data.Id == checkedRecord.data.Id })
        });
        // не выбранные
        var uncheckedPromoessInGrid = promoRecords.filter(function (record) {
            return !checkedRecords.includes(record)
        });
        if (checkedPromoesInGrid.length > 10) {
            App.Notify.pushError('Promoes more than 10 selected');
            return;
        }
        checkedPromoesInGrid.forEach(function (promo) {
            var storeproduct = promoStore.findRecord('Id', promo.data.Id);
            if (storeproduct.data.MasterPromoId != widget.PromoId) {
                storeproduct.set('MasterPromoId', widget.PromoId)
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
        promoStore.save({
            scope: this,
            success: function (rec, resp, opts) {
                widget.setLoading(false);
                widget.close();
            },
            failure: function () {
                widget.setLoading(false);
            }
        });
    },
});