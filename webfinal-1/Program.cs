var builder = WebApplication.CreateBuilder(args);

// 加入 MVC 與 Session 支援
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // ✅ 加入 Session

var app = builder.Build();

// 中介層設定
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        // ✅ 啟用 Session（放在 UseRouting 後）
app.UseAuthorization();

// ✅ 將首頁導向 Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();