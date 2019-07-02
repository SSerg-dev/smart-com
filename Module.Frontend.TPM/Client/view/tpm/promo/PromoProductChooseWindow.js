﻿Ext.define('App.view.tpm.promo.PromoProductChooseWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promoproductchoosewindow',

    width: 1100,
    minWidth: 280,
    resizable: false,
    title: l10n.ns('tpm', 'PromoBasicProducts').value('Products'),

    // ObjectID изначально выбранных (чекнутых) продуктов
    choosenProductObjectIds: [],
    // Есть ли блокировка по датам
    treesChangingBlockDate: null,
    // Функция при смене клиента
    callBackChooseFnc: null,

    initComponent: function () {
        this.callParent(arguments);

        this.add({
            xtype: 'producttree',
            header: false,
            height: 547,
            minHeight: 547,
            maxHeight: 547,
            chooseMode: true,
            needLoadTree: false,
            hideNotHierarchyBtns: true
        });

        var productTreeStore = this.down('producttreegrid').store;
        var productTreeProxy = productTreeStore.getProxy();

        if (this.choosenProductObjectIds.length > 0) {
            var me = this;
            var choosenObjectIdsString = '';
            this.choosenProductObjectIds.forEach(function (item, index) {
                choosenObjectIdsString += item;

                if (index != me.choosenProductObjectIds.length - 1) {
                    choosenObjectIdsString += ';';
                }
            });

            productTreeProxy.extraParams.productTreeObjectIds = choosenObjectIdsString;
        }

        if (this.treesChangingBlockDate) {
            productTreeProxy.extraParams.dateFilter = treesChangingBlockDate;
        } else {
            productTreeProxy.extraParams.dateFilter = null;
        }

        productTreeStore.load();
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('tpm', 'PromoBasicProducts').value('ChooseBtn'),
        itemId: 'choose',
        style: { "background-color": "#26A69A" }
    }]
})