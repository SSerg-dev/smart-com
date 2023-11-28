Ext.define('App.controller.tpm.actualtradeinvestment.ActualTradeInvestment', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'actualtradeinvestment[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'actualtradeinvestment directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'actualtradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'actualtradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'actualtradeinvestment #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'actualtradeinvestment #detailform #next': {
                    click: this.onNextButtonClick
                },
                'actualtradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'actualtradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'actualtradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'actualtradeinvestment #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'actualtradeinvestment #createbutton': {
                    click: this.onCreateButtonClick
                },
                'actualtradeinvestment #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'actualtradeinvestment #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'actualtradeinvestment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'actualtradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'actualtradeinvestment #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'actualtradeinvestment #exportbutton': {
                    click: this.onExportButtonClick
                },
                'actualtradeinvestment #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'actualtradeinvestment #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'actualtradeinvestment #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                //'actualtradeinvestment #recalculateactualtipreviousyearbutton': {
                //    click: this.onRecalculatePreviousYearButtonClick
                //},

                '#confirmPromoRecalculationTIWindow #confirm': {
                    click: this.onConfirmPromoRecalculationButtonClick
                }
            }
        });
    },

    onGridAfterrender: function (grid) {
        //this.callParent(arguments);

        //var resource = 'ActualTradeInvestments',
        //    action = 'IsTIRecalculatePreviousYearButtonAvailable',
        //    allowedActions = [];

        //var currentRoleAPs = App.UserInfo.getCurrentRole().AccessPoints.filter(function (point) {
        //    return point.Resource === resource;
        //});

        //currentRoleAPs.forEach(function (point) {
        //    if (!allowedActions.some(function (action) { return action === point.Action })) {
        //        allowedActions.push(point.Action);
        //    }
        //});

        //if (Ext.Array.contains(allowedActions, action)) {
        //    var store = grid.getStore();
        //    store.on('load', function (store) {
        //        parameters = {};

        //        App.Util.makeRequestWithCallback(resource, action, parameters, function (data) {
        //            if (data) {
        //                var result = Ext.JSON.decode(data.httpResponse.data.value);
        //                if (result.success) {
        //                    var recalculatePreviousYearButton = grid.up('panel').down('#recalculateactualtipreviousyearbutton');
        //                    this.recalculatePreviousYearButton = recalculatePreviousYearButton;
        //                    if (result.isRecalculatePreviousYearButtonAvailable) {
        //                        recalculatePreviousYearButton.setDisabled(false);
        //                    } else {
        //                        recalculatePreviousYearButton.setDisabled(true);
        //                    }
        //                } else {
        //                    App.Notify.pushError(data.message);
        //                }
        //            }
        //        }, function (data) {
        //            App.Notify.pushError(data.message);
        //        });
        //    });
        //}
    },

    onRecalculatePreviousYearButtonClick: function (button) {
        var actualTIPanel = button.up('actualtradeinvestment'),
            parameters = {};

        actualTIPanel.setLoading(true);

        App.Util.makeRequestWithCallback('ActualTradeInvestments', 'PreviousYearPromoList', parameters, function (data) {
            if (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    var previousYearPromoes = result.promoes;

                    var simplePromoViewerWidget = Ext.widget('basereviewwindow', {
                        title: l10n.ns('core').value('confirmTitle'),
                        itemId: 'confirmPromoRecalculationTIWindow',
                        width: '55%',
                        height: '75%',
                        minHeight: 450,
                        minWidth: 600,

                        items: {
                            xtype: 'simplepromoviewer',
                            title: 'Promo for night recalculation'
                        },
                        buttons: [{
                            text: l10n.ns('core', 'buttons').value('confirm'),
                            itemId: 'confirm'
                        }, {
                            text: l10n.ns('core', 'buttons').value('close'),
                            itemId: 'close'
                        }]
                    });

                    var grid = simplePromoViewerWidget.down('simplepromoviewer').down('directorygrid'),
                        store = grid.getStore(),
                        proxy = store.getProxy();

                    var promoArray = new Array();
                    previousYearPromoes.forEach(function (item) {
                        var model = Ext.create('App.model.tpm.promo.SimplePromo');
                        model.data = item;
                        promoArray.push(model);
                    });

                    proxy.data = promoArray;
                    store.load();

                    simplePromoViewerWidget.show();
                    actualTIPanel.setLoading(false);
                } else {
                    actualTIPanel.setLoading(false);
                    App.Notify.pushError(data.message);
                }
            }
        }, function (data) {
            actualTIPanel.setLoading(false);
            App.Notify.pushError(data.message);
        });
    },

    onConfirmPromoRecalculationButtonClick: function (button) {
        var window = button.up('#confirmPromoRecalculationTIWindow'),
            parameters = {};

        window.setLoading(true);

        App.Util.makeRequestWithCallback('ActualTradeInvestments', 'CreateActualTIChangeIncidents', parameters, function (data) {
            window.setLoading(false);
            this.recalculatePreviousYearButton.setDisabled(true);
            window.close();
        }, function (data) {
            window.setLoading(false);
            if (data) {
                App.Notify.pushError(data.message);
            }
        })
    },

    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
        }
    }
});