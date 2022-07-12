Ext.define('App.view.core.modelswitcher.ModeSwitcher', {
    extend: 'Ext.Component',
    alias: 'widget.modelswitcher',
    autoEl: {
        tag: 'input',
        type: 'checkbox',
        cls: 'modelswitchercheckbox',
        name: 'topping'
    },
    listeners: {
        afterrender: function (inputCmp) {
            
            inputCmp.mon(inputCmp.el, 'change', function () {
                debugger;
                var setting = App.util.core.Setting;
                if (inputCmp.el.dom.checked) {
                    setting.mode = 1;
                    //console.log("Mode RS");
                } else {
                    setting.mode = 0;
                    //console.log("Mode Standart");
                }
                //alert('click!')
            }, this);
        }
        , single: true
    }
});