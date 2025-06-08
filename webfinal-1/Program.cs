var builder = WebApplication.CreateBuilder(args);

// 加入 MVC 與 Session 支援
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // ✅ 這裡加入 Session

var app = builder.Build();

// 開始中介層設定
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();        // ✅ 啟用 Session（一定要放在 UseRouting 之後）
app.UseAuthorization();

// 設定路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SelectZone}/{id?}");

app.Run();
