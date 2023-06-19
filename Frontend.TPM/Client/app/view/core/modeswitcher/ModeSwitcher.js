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
            if (TpmModes.isRsRaMode()) {
                inputCmp.el.dom.checked = true;
            }

            inputCmp.mon(inputCmp.el, 'change', function () {
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
                    TpmModes.setMode(TpmModes.Prod.id);
                    window.location.reload();
                    //MenuMgr.refreshCurrentMenu();
                    //alert('click!')
                    
                    //console.log("Mode Standart");
                }

                function onMsgBoxClose(buttonId) {
                    if (buttonId === 'yes') {
                        TpmModes.setMode(TpmModes.RS.id);
                    } else {
                        TpmModes.setMode(TpmModes.RA.id);
                    }
                    window.location.reload();
                    //MenuMgr.refreshCurrentMenu();
                    //alert('click!')
                }                
            }, this);
        }
        , single: true
    }
});