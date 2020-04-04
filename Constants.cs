namespace Test
{
    public static class Constants
    {
        public const string AllowAnyUserIp = "*";
        public const string ContentNotificationsApiEndpoint = "api/v1/contentnotifications/";
        public const int FallbackLanguageLcid = 1033; // EN-US
        public const string FallbackLanguageCulture = "en-us";
        public const int MaxNumberOfRowsFromApi = 100;
        public const string AzureBlobSharedContainerName = "shared";
        public const string AzureBlobPortalContainer = "portal";
        public const string AzureBlobContentContainer = "content";
        public const string AzureBlobNotificationContainer = "notification";
        public const string AzureBlobExportContainerName = "export";
        public const int UserNameLength = 8;
        public const int PasswordLength = 32;
        public const int NoNodeId = -1;
        public const int NoContentId = -2;
        public const int NoPortalId = -3;
        public const int NoUserId = -4;
        public const int NoUserIntId = -4;
        public const int NoCustomerId = -5;
        public const string NoUserIp = "-6";
        public const int NoRole = 0;
        public const long NoUserUgam = 0;
        public const string CurrentPortalIdHeader = "X-Current-Portal-Id";
        public const string CustomCurrentIpHeader = "X-Original-Client-Ip";
        public const string CustomPlatformNameHeader = "X-Platform-Name";
        public const string ResponseTypeHeader = "X-Response-Type";
        public const int NoTranslationId = -6;
        public const int MaxSearchQueryLenghtInStatistics = 100;
        public const int MostReadExpireInMinutes = 5;
        public const int SearchSnippetCharLength = 400;
        public const int MostReadTakeDefault = 5;
        public const int MostReadTakeExtraBeforeFilter = 5;
        public const int MostReadNumberOfDays = 30;
        public const int MaxNumberOfNewsAndRecommended = 15;
        public const int DefaultSearchItemsPerPage = 10;
        public const string ComaroundSmallIcon = "/images/comaround-small.png";
        public const string BreaklineTagForWordImpport = "<br/>";
        public const int AzureSearchExpRetryAttemps = 4; // Wait for a max total of 30s 
        public const int BatchSizeAzure = 500;
        public const int BatchSizeRedis = 100;
        public const int BatchSizeServiceBus = 300;
        public const int BatchSizeStatisticsDb = 250;
        public const int BatchSizeAzureSearchFilter = 50;
        public const string ZeroImageTag = "data-zero-img";
        public const string ZeroHyperlinkTag = "data-zero-a";
        public const string ZeroVideoTag = "data-zero-video";
        public const string ZeroObjectTag = "data-zero-object";
        public const int HelpFolderNodeId = 320586;
        public const int GettingStartedNodeId = 529639;
        public const int OldSharedComaroundNodeId = 170665;
        public const int SharedComaroundNodeId = 1063976;
        public const int DefaultMaxNodes = 25;
        public const string ArticleHistoryTableName = "articlehistory";
        public const string DecisionTreeTableName = "decisiontree";
        public const string DecisionTreeSetTableName = "decisiontreeset";
        public const string DefaultBannerColor = "#BF242C";
        public const string DefaultBannerTextColor = "#FFFFFF";
        public const string DefaultMobileBannerTextColor = "#000000";
        public const string DefaultSearchTextColor = "#BF242C";
        public const string DefaultBackgroundImageOverlayColor = "#FFFFFF";
        public const int MaxSearchQueryLength = 100;
        public const int MaxTableServiceStringPropertySizeInBytes = 63900; // https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model
        public const string CulturePattern = @"^(?<language>[a-zA-Z]{2,3})(-(?<script>[a-zA-Z]{4}))?(-(?<region>[a-zA-Z]{2,5}))$";
        public const string UrlPattern = @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
        public const string HexColorPattern = @"^#(?:[0-9a-fA-F]{3}){1,2}$";
        public const string ZeroContentRelativePath = "/content/";
        public const string SelfServiceArticleRelativePath = "/article/";
        public static readonly int[] AppIconSizes = new[] { 16, 32, 48, 72, 96, 128, 144, 152, 192, 384, 512 };
        public const int PortalTagsExpireInMinutes = 60 * 24; // Cache for one day since it will get invalidated when deleting translations anyway

        #region CacheKeys
        public const string CacheKeyForNode = "portal:{0}:node:{1}";
        public const string CacheKeyForContent = "content:{0}";
        public const string CacheKeyForCustomer = "customer:{0}:customer";
        public const string CacheKeyForPortal = "portal:{0}:portal";
        public const string CacheKeyForMostRead = "portal:{0}:mostrd:days{1}.lcid{2}.take{3}.statstrat.{4}";
        public const string CacheKeyForMostReadStartPattern = "portal:{0}:mostrd:";
        public const string CacheKeyForViewsAndVotes = "portal:{0}:content:{1}.statstrat.{2}";
        public const string CacheKeyForPortalAllowedCultures = "portal:{0}:lang:allowed.cult:";
        public const string CacheKeyForPortalDefaultLanguage = "portal:{0}:lang:default:";
        public const string CacheKeyForPortalTags = "portal:{0}:tags";
        public const string CacheKeyForSystemAllowedCultures = "system:lang:allowed.cult";
        public const string CacheKeyForCustomerOverviews = "allPortals";
        public const string CacheKeyForFileStatus = "content:{0}:filename:{1}";
        #endregion

        #region Values used for Internal Claims
        public const string AllowedIpClaimType = "allowedip";
        public const string CurrentPortalIdClaimType = "currentportalid";
        public const string CustomerIdClaimType = "customerid";
        public const string PortalIdsClaimType = "portalids";
        public const string UserUgamClaimType = "userugam";
        public const string UserIdClaimType = "sub";
        public const string RoleClaimType = "role";
        public const string UserAuthenticationMethodClaimType = "amr";
        public const string NameClaimType = "name";
        public const string EmailClaimType = "email";
        public const string IdTokenClaimType = "id_token";
        public const string TenantClaimType = "tennant";
        public const string GroupsClaimType = "groups";
        public const string UserGroupIdType = "usergroupids";
        public const string SubscriptionClaimType = "subscription";

        #endregion
    }

}