Ext.define('App.util.core.ShareTemplateHelper', {

    statics: {
        showSelectTemplateWindow: function (resource, action, panel, testId, rec, pTestProducts, windowName) {
            var maxWindowHeight = window.innerHeight * 0.95;
            var gridHeight = (maxWindowHeight - 150) / 2;
            var gridCfg = {
                minHeight: 170,
                height: Math.min(gridHeight, 325)
            };
            var localProductWidget = Ext.widget('localtestproduct', gridCfg);
            var localProductsStore = localProductWidget.down('directorygrid').getStore();
            var localShareWidget = Ext.widget('localtestptestproduct', gridCfg);
            var localStore = localShareWidget.down('directorygrid').getStore();

            var editor = Ext.create('App.view.core.common.EditorWindow', {
                title: 'Распределить по шаблону',
                name: windowName,
                width: 1200,
                maxHeight: maxWindowHeight,
                grid: panel,
                resource: resource,
                action: action,
                record: rec,
                items: [
                    Ext.widget('selectproductsharetemplateform'),
                    localShareWidget,
                    localProductWidget
                ],
                buttons: [{
                    text: l10n.ns('core', 'buttons').value('cancel'),
                    itemId: 'close'
                }, {
                    text: l10n.ns('core', 'buttons').value('ok'),
                    ui: 'green-button-footer-toolbar',
                    itemId: 'apply'
                }]
            });

            var field = editor.down('searchfield[name=TemplateId]');
            field.pTestProductStore = localStore;
            panel.setLoading(true);
            editor.on('close', function () {
                panel.setLoading(false);
                panel.down('directorygrid').getStore().load();
            });

            localProductsStore.setFixedFilter('TestId', {
                property: 'TestId',
                operation: 'Equals',
                value: testId
            });
            localProductsStore.on({
                single: true,
                load: function (records, operation, success) {
                    if (success) {
                        // panel.setLoading(false);
                        var count = records.getCount();
                        field.selectorWidgetConfig = {
                            productList: count > 0 ? records.getRange(0, count) : []
                        };
                        localStore.setProxy({
                            type: 'memory',
                            data: pTestProducts
                        });
                        localStore.load();
                        editor.show();
                    } else {
                        panel.setLoading(false);
                        App.Notify.pushError('Ошибка при получении тестируемых продуктов');
                    }
                }
            });
            localProductsStore.load();
        }
    }

});