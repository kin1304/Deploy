// Xử lý đăng xuất khi người dùng nhấn nút
document.getElementById('logoutBtn').addEventListener('click', function () {
    // Gửi yêu cầu đăng xuất tới server
    fetch('/Account/Logout', {
        method: 'POST', // Sử dụng POST để gọi API Logout
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (response.ok) {
                // Chuyển hướng về trang đăng nhập sau khi đăng xuất thành công
                window.location.href = '/Account/Login';
            } else {
                alert('Có lỗi xảy ra trong quá trình đăng xuất');
            }
        })
        .catch(error => {
            console.error('Lỗi:', error);
        });
});
