Ext.define('App.controller.tpm.promo.UserRolePromo', {
    extend: 'App.controller.core.AssociatedDirectory',
    init: function () {
        this.listen({
            component: {
                'userrolepromo[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick,
                },
                'userrolepromo directorygrid': {
                    selectionchange: this.onUserRoleGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'userrolepromo #datatable': {
                    activate: this.onActivateCard
                },
                'userrolepromo #detailform': {
                    activate: this.onActivateCard
                },
                'userrolepromo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'userrolepromo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'userrolepromo #detail': {
                    click: this.switchToDetailForm
                },
                'userrolepromo #table': {
                    click: this.onTableButtonClick
                },
                'userrolepromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'userrolepromo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'userrolepromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'userrolepromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'userrolepromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    },

    onUserRoleGridSelectionChange: function (selModel, selected) {
        this.onGridSelectionChange(selModel, selected);
        selModel.view.up('selectorwindow').down('#select').setDisabled(false);
    },

    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            window = button.up('selectorwindow'),
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