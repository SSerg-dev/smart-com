Ext.define('App.view.tpm.promo.PromoClientChooseWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promoclientchoosewindow',

    width: 1000,
    minWidth: 280,
    resizable: false,
    title: l10n.ns('tpm', 'PromoClient').value('Clients'),

    // Object ID изначально выбранного (чекнутого) клиента
    choosenClientObjectId: null,
    // Есть ли блокировка по датам
    treesChangingBlockDate: null,
    // Функция при смене клиента
    callBackChooseFnc: null,

    initComponent: function () {
        this.callParent(arguments);

        this.add({
            xtype: 'clienttree',
            header: false,
            height: 547,
            minHeight: 547,
            maxHeight: 547,
            chooseMode: true,
            choosenClientObjectId: this.choosenClientObjectId,
            needLoadTree: false,
            hideNotHierarchyBtns: true
        });

        var clientTreeStore = this.down('clienttreegrid').store;
        var clientTreeProxy = clientTreeStore.getProxy();

        if (this.choosenClientObjectId != null) {
            clientTreeProxy.extraParams.clientObjectId = this.choosenClientObjectId;
        }

        if (this.treesChangingBlockDate) {
            clientTreeProxy.extraParams.dateFilter = treesChangingBlockDate;
        } else {
            clientTreeProxy.extraParams.dateFilter = null;
        }

        clientTreeStore.load();
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('tpm', 'PromoClient').value('ChooseBtn'),
        itemId: 'choose',
        style: { "background-color": "#26A69A" }
    }]
})