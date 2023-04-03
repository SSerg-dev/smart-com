UserRoles = {
    KeyAccountManager: 'KAM',
    CustomerMarketingManager: 'CMManager',
    DemandPlanning: 'DemandPlanning',
    DemandFinance: 'DemandFinance',
    Administrator: 'Administrator',
    SupportAdministrator: 'SupportAdministrator',
    FunctionalExpert: 'FunctionalExpert',
    CustomerMarketing: 'CustomerMarketing',
    GrowthAccelerationManager: 'GAManager',
    SuperReader: 'SuperReader',
    
    isKeyAccountManager: function(roleName) {
        return roleName === this.KeyAccountManager;
    },
    isCustomerMarketingManager: function(roleName) {
        return roleName === this.CustomerMarketingManager;
    },
    isDemandPlanning: function(roleName) {
        return roleName === this.DemandPlanning;
    },
    isDemandFinance: function(roleName) {
        return roleName === this.DemandFinance;
    },
    isAdministrator: function(roleName) {
        return roleName === this.Administrator;
    },
    isSupportAdministrator: function(roleName) {
        return roleName === this.SupportAdministrator;
    },
    isFunctionalExpert: function(roleName) {
        return roleName === this.FunctionalExpert;
    },
    isCustomerMarketing: function(roleName) {
        return roleName === this.CustomerMarketing;
    },
    isGrowthAccelerationManager: function(roleName) {
        return roleName === this.GrowthAccelerationManager;
    },
    isSuperReader: function(roleName) {
        return roleName === this.SuperReader;
    },
    
    isMassApproveRole: function(roleName) {
        return [this.CustomerMarketingManager, this.DemandPlanning].indexOf(roleName) >= 0;
    } 
};