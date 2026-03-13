using System;
using GOA.Config;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using Rubik.IAP;
using Rubik.UI;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;


public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    IStoreController m_StoreController; // The Unity Purchasing system.
    IExtensionProvider m_StoreExtensionProvider; // The Unity Purchasing system extension.

    //Your products IDs. They should match the ids of your products in your store.

    string[] ProductsID = {
        "com.rubik.myrk.starterpack",
        "com.rubik.myrk.wepon1",
        "com.rubik.myrk.armor1",
        "com.rubik.myrk.hair1",
        "com.rubik.myrk.wepon2",
        "com.rubik.myrk.armor2",
        "com.rubik.myrk.hair2",
        "com.rubik.myrk.wepon3",
        "com.rubik.myrk.armor3",
        "com.rubik.myrk.hair3",
        "com.rubik.myrk.gem1",
        "com.rubik.myrk.gem2",
        "com.rubik.myrk.gem3",

        IAP_Config.IAP_BattlePass1,
        IAP_Config.IAP_RemoveAds,
    };

    public static IAPManager Instance;
    void Start()
    {
        Instance = this;
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        //Add products that will be purchasable and indicate its type.

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < ProductsID.Length; i++)
        {
            builder.AddProduct(ProductsID[i], ProductType.Consumable);
        }
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProduct(string _productID)
    {
        HUDCanvas.Instance.ShowLoadingPanel();
        m_StoreController.InitiatePurchase(_productID);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        //Add the purchased product to the players inventory
        // if (product.definition.id == "gold_1")
        // {

        // }

        // Pub mesg success
        HUDCanvas.Instance.HideLoadingPanel();
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            IAP_Controller.Instance.PurchaseSuccess(product.definition.id, args.purchasedProduct.transactionID, args.purchasedProduct.appleProductIsRestored);
            if (!args.purchasedProduct.appleProductIsRestored)
            {
                AppsFlyerManager.TrackingPurchases(product.definition.id,"ios");
            }
        }else{
            IAP_Controller.Instance.PurchaseSuccess(product.definition.id, args.purchasedProduct.transactionID, false);
            AppsFlyerManager.TrackingPurchases(product.definition.id, "android");
        }


        Debug.Log($"Purchase Complete - Product: {product.definition.id}" + "=== Price : " + product.metadata.localizedPriceString);

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        HUDCanvas.Instance.HideLoadingPanel();
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        IAP_Controller.Instance.PurchaseFailed(product.definition.id);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        HUDCanvas.Instance.HideLoadingPanel();
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }

    public string getPriceProduct(string _idProduct)
    {
        var _product = m_StoreController.products.WithStoreSpecificID(_idProduct);
        if(_product == null){
            Debug.LogError("Product not found: " + _idProduct);
            return "0";
        }
        return _product.metadata.localizedPriceString + " " + _product.metadata.isoCurrencyCode;
    }

    public Product GetProductByID(string idProduct){
        return m_StoreController.products.WithStoreSpecificID(idProduct);
    }
    
    public void RestorePurchases()
    {
            NTLog.LogMessage("Bắt đầu quá trình khôi phục giao dịch...");
        // Chỉ thực hiện trên nền tảng của Apple (iOS, macOS).
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            
            // Lấy extension của Apple.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            
            // Gọi hàm RestoreTransactions. 
            // Quá trình này sẽ kích hoạt lại hàm ProcessPurchase cho mỗi vật phẩm non-consumable mà người dùng đã mua.
            apple.RestoreTransactions((result, error) => {
                // Callback này sẽ được gọi khi quá trình hoàn tất.
                // Bạn có thể hiển thị một thông báo cho người dùng ở đây.
                if (result)
                {
                    // HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_success", "Restore Success!"));
                    NTLog.LogMessage("Khôi phục thành công!" + error);
                    // Ví dụ: Hiển thị popup "Các giao dịch của bạn đã được khôi phục thành công."
                }
                else
                {
                    NTLog.LogMessage("Khôi phục thất bại." + error);
                    // Ví dụ: Hiển thị popup "Khôi phục thất bại. Vui lòng thử lại."
                }
            });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            var google = m_StoreExtensionProvider.GetExtension<IGooglePlayStoreExtensions>();
            google.RestoreTransactions((result, error) => {
                if (result)
                {
                    // HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_success", "Restore Success!"));
                    NTLog.LogMessage("Khôi phục thành công!" + error);
                }
                else
                {
                    NTLog.LogMessage("Khôi phục thất bại." + error);
                }
            });
        }else{
            HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_success", "Restore Success!"));
        }
    }
}

