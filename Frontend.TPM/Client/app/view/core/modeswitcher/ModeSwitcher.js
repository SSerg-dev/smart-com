Ext.define('App.view.core.modelswitcher.ModeSwitcher', {
    extend: 'Ext.Component',
    alias: 'widget.modelswitcher',
    autoEl: {
        tag: 'input',
        type: 'checkbox',
        cls: 'modelswitchercheckbox',
        name: 'topping',
        id: 'modelswitchercheckboxid'
    },
    listeners: {
        afterrender: function (inputCmp) {
            var settingStore = Ext.create('App.store.core.settinglocal.SettingLocalStore');
            settingStore.load();
            var mode = settingStore.findRecord('name', 'mode');
            if (mode) {
                if (mode.data.value == 1) {
                    inputCmp.el.dom.checked = true;
                }
            }
            inputCmp.mon(inputCmp.el, 'change', function () {
                var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
                settingStore.load();
                var mode = settingStore.findRecord('name', 'mode');
                if (inputCmp.el.dom.checked) {
                    mode.set('value', 1);
                    //console.log("Mode RS");
                } else {
                    mode.set('value', 0);
                    //console.log("Mode Standart");
                }
                settingStore.sync();
                MenuMgr.refreshCurrentMenu();
                //alert('click!')
            }, this);
        }
        , single: true
    }
});