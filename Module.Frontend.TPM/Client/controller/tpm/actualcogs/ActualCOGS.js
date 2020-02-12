Ext.define('App.controller.tpm.actualcogs.ActualCOGS', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'actualcogs[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'actualcogs directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'actualcogs #datatable': {
                    activate: this.onActivateCard
                },
                'actualcogs #detailform': {
                    activate: this.onActivateCard
                },
                'actualcogs #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'actualcogs #detailform #next': {
                    click: this.onNextButtonClick
                },
                'actualcogs #detail': {
                    click: this.switchToDetailForm
                },
                'actualcogs #table': {
                    click: this.onTableButtonClick
                },
                'actualcogs #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'actualcogs #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'actualcogs #createbutton': {
                    click: this.onCreateButtonClick
                },
                'actualcogs #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'actualcogs #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'actualcogs #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'actualcogs #refresh': {
                    click: this.onRefreshButtonClick
                },
                'actualcogs #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'actualcogs #exportbutton': {
                    click: this.onExportButtonClick
                },
                'actualcogs #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'actualcogs #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'actualcogs #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                'actualcogs #recalculateactualcogspreviousyearbutton': {
                    click: this.onRecalculatePreviousYearButtonClick
                },

                '#confirmPromoRecalculationCOGSWindow #confirm': {
                    click: this.onConfirmPromoRecalculationButtonClick
                }
            }
        });
    },

    onGridAfterrender: function (grid) {
        this.callParent(arguments);
        
        var resource = 'ActualCOGSs',
            action = 'IsCOGSRecalculatePreviousYearButtonAvailable',
            allowedActions = [];

        var currentRoleAPs = App.UserInfo.getCurrentRole().AccessPoints.filter(function (point) {
            return point.Resource === resource;
        });

        currentRoleAPs.forEach(function (point) {
            if (!allowedActions.some(function (action) { return action === point.Action })) {
                allowedActions.push(point.Action);
            }
        });

        if (Ext.Array.contains(allowedActions, action)) {
            var store = grid.getStore();
            store.on('load', function (store) {
                parameters = {};

                App.Util.makeRequestWithCallback(resource, action, parameters, function (data) {
                    if (data) {
                        var result = Ext.JSON.decode(data.httpResponse.data.value);
                        if (result.success) {
                            var recalculatePreviousYearButton = grid.up('panel').down('#recalculateactualcogspreviousyearbutton');
                            this.recalculatePreviousYearButton = recalculatePreviousYearButton;
                            if (result.isRecalculatePreviousYearButtonAvailable) {
                                recalculatePreviousYearButton.setDisabled(false);
                            } else {
                                recalculatePreviousYearButton.setDisabled(true);
                            }
                        } else {
                            App.Notify.pushError(data.message);
                        }
                    }
                }, function (data) {
                    App.Notify.pushError(data.message);
                });
            });
        }
    },

    onRecalculatePreviousYearButtonClick: function (button) {
        var actualCogsPanel = button.up('actualcogs'),
            parameters = {};

        actualCogsPanel.setLoading(true);

        App.Util.makeRequestWithCallback('ActualCOGSs', 'PreviousYearPromoList', parameters, function (data) {
            if (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    var previousYearPromoes = result.promoes;

                    var simplePromoViewerWidget = Ext.widget('basereviewwindow', {
                        title: l10n.ns('core').value('confirmTitle'),
                        itemId: 'confirmPromoRecalculationCOGSWindow',
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
                    actualCogsPanel.setLoading(false);
                } else {
                    actualCogsPanel.setLoading(false);
                    App.Notify.pushError(data.message);
                }
            }
        }, function (data) {
            actualCogsPanel.setLoading(false);
            App.Notify.pushError(data.message);
        });
    },

    onConfirmPromoRecalculationButtonClick: function (button) {
        var window = button.up('#confirmPromoRecalculationCOGSWindow'),
            parameters = {};

        window.setLoading(true);

        App.Util.makeRequestWithCallback('ActualCOGSs', 'CreateActualCOGSChangeIncidents', parameters, function (data) {
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

            store.setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(selModel.getSelection()[0])
            });
        }
    }
});
