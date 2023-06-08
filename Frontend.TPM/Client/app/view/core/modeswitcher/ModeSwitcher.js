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
                    Ext.Msg.show({
                        title: 'Choose mode',
                        msg: 'Choose RS and RA mode',
                        fn: onMsgBoxClose,
                        scope: this,
                        buttons: Ext.Msg.YESNO,
                        buttonText: {
                            yes: 'RS',
                            no: 'RA'
                        }
                    });
                    //console.log("Mode RS");
                } else {
                    mode.set('value', 0);

                    settingStore.sync();
                    window.location.reload();
                    //MenuMgr.refreshCurrentMenu();
                    //alert('click!')
                    
                    //console.log("Mode Standart");
                }

                function onMsgBoxClose(buttonId) {
                    if (buttonId === 'yes') {
                        mode.set('value', 1);
                    } else {
                        mode.set('value', 2);
                    }
                    settingStore.sync();
                    window.location.reload();
                    //MenuMgr.refreshCurrentMenu();
                    //alert('click!')
                }                
            }, this);
        }
        , single: true
    }
});