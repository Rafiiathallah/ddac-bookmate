using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ddac_bookmate.Services;

namespace ddac_bookmate.Services
{
    public class SNSService : ISNSService
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly string _topicArn;
        private readonly ILogger<SNSService> _logger;

        public SNSService(IConfiguration configuration, ILogger<SNSService> logger)
        {
            _logger = logger;
            var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);
            _snsClient = new AmazonSimpleNotificationServiceClient(region);
            _topicArn = configuration["AWS:SNSTopicArn"];
            
            _logger.LogInformation($"SNS Service initialized with TopicArn: {_topicArn}");
        }

        public async Task<bool> PublishMessageAsync(string message, string subject, string userEmail = null)
        {
            try
            {
                _logger.LogInformation($"[SNS-Notification] Start - Sending to {userEmail}");
                _logger.LogInformation($"[SNS-Notification] TopicArn: {_topicArn}");
                
                // Check topic attributes
                try
                {
                    var topicAttrs = await _snsClient.GetTopicAttributesAsync(_topicArn);
                    _logger.LogInformation($"[SNS-Notification] Topic exists with {topicAttrs.Attributes["SubscriptionsConfirmed"]} confirmed subscriptions");
                }
                catch (Exception topicEx)
                {
                    _logger.LogError($"[SNS-Notification] Topic Error: {topicEx.Message}");
                    return false;
                }

                var messageBody = new
                {
                    Default = message,
                    Email = $"Hello {userEmail},\n\n{message}\n\nBest regards,\nBookmate Team"
                };

                _logger.LogInformation($"[SNS-Notification] Message Body: {JsonSerializer.Serialize(messageBody)}");

                var request = new PublishRequest
                {
                    TopicArn = _topicArn,
                    Message = JsonSerializer.Serialize(messageBody),
                    Subject = subject,
                    MessageStructure = "json"
                };

                var response = await _snsClient.PublishAsync(request);
                _logger.LogInformation($"[SNS-Notification] Success - MessageId: {response.MessageId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[SNS-Notification] Error: {ex.Message}");
                _logger.LogError($"[SNS-Notification] Stack: {ex.StackTrace}");
                return false;
            }
        }
    }
} 