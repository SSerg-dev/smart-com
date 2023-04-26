Ext.define('App.controller.tpm.planpostpromoeffect.PlanPostPromoEffect', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'planpostpromoeffect[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'planpostpromoeffect directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'planpostpromoeffect #datatable': {
                    activate: this.onActivateCard
                },
                'planpostpromoeffect #detailform': {
                    activate: this.onActivateCard
                },
                'planpostpromoeffect #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'planpostpromoeffect #detailform #next': {
                    click: this.onNextButtonClick
                },
                'planpostpromoeffect #detail': {
                    click: this.switchToDetailForm
                },
                'planpostpromoeffect #table': {
                    click: this.onTableButtonClick
                },
                'planpostpromoeffect #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'planpostpromoeffect #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'planpostpromoeffect #createbutton': {
                    click: this.onCreateButtonClick
                },
                'planpostpromoeffect #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'planpostpromoeffect #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'planpostpromoeffect #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'planpostpromoeffect #refresh': {
                    click: this.onRefreshButtonClick
                },
                'planpostpromoeffect #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'planpostpromoeffect #exportbutton': {
                    click: this.onExportButtonClick
                },
                'planpostpromoeffect #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'planpostpromoeffect #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'planpostpromoeffect #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
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
    },

    getBrandTechSizes: function(brandTechCode) {
        var comboSize = Ext.ComponentQuery.query('#SizeComboBox')[0];
        var store = comboSize.getStore();
        store.clearData();

        var params = {
            brandTechCode: brandTechCode
        }
        if (params.brandTechCode) {
            App.Util.makeRequestWithCallback('PlanPostPromoEffects', 'GetBrandTechSizes', params, function (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value).data;
                for (var i = 0; i < result.length; i++) {
                    store.add({size: result[i]});
                }
            });
        }
    }
});
